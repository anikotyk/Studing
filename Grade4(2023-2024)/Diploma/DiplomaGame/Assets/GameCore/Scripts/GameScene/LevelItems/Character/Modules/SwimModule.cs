using DG.Tweening;
using EPOOutline;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Sensors;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Character.Modules
{
    public class SwimModule : InteractorCharacterModule
    {
        [InjectOptional, UsedImplicitly] public Sea sea { get; }
        
        [SerializeField] private AnimatorParameterApplier _swim;
        [SerializeField] private AnimatorParameterApplier _notSwim;
        [SerializeField] private AnimatorParameterApplier _jumpToWater;
        [SerializeField] private AnimatorParameterApplier _stopJumpToWater;
        [SerializeField] private GameObject _swimTools;
        [SerializeField] private Transform _pointForRaycast;
        [SerializeField] private Transform _pointForRaycastPlot;
        [SerializeField] private Transform _pointForRaycastFloor;
        [SerializeField] private bool _isToAnimateStartEndSwim;
        [SerializeField] private ParticleSystem _vfxSwim;
        [SerializeField] private ParticleSystem _vfxIdleSwim;
        [SerializeField] private ParticleSystem _vfxJump;
        [SerializeField] private AudioSource _soundJump;
        [SerializeField] private Outlinable _outlinable;
        [ColorUsage(true, true)]
        [SerializeField] private Color _swimColor;
        [ColorUsage(true, true)]
        [SerializeField] private Color _notSwimColor;
        
        private bool _isSwim = false;
        public bool isSwim => _isSwim;

        private bool _isJumpingToWater;

        private Sequence _containerMoveSequence;
        private LocomotionCharacterMovingModule _characterMovingModule;

        private Rigidbody _rb;

        private Tween _jumpTween;
        private Tween _jumpDelayedCall;

        private bool _blockRaycast = false;
        private bool _goingToGround = false;
        
        public readonly TheSignal onSwim = new();
        public readonly TheSignal onNotSwim = new();
        
        public override void Construct()
        {
            base.Construct();
            _characterMovingModule = character.GetModule<LocomotionCharacterMovingModule>();
            _rb = character.GetComponent<Rigidbody>();
            _characterMovingModule.onStartMoving.On(() =>
            {
                if (_vfxIdleSwim != null)
                {
                    _vfxIdleSwim.Stop();
                }
                
                if (isSwim)
                {
                    _vfxSwim.gameObject.SetActive(true);
                    _vfxSwim.Play();
                }
            });
            
            _characterMovingModule.onStopMoving.On(() =>
            {
                _vfxSwim.Stop();
                if (isSwim)
                {
                    if (_vfxIdleSwim != null)
                    {
                        _vfxIdleSwim.gameObject.SetActive(true);
                        _vfxIdleSwim.Play();
                    }
                }
            });
        }

        public void SetSwim()
        {
            if (_goingToGround) return;
            if(_isSwim) return;
            
            if (_isJumpingToWater)
            {
                _vfxJump.gameObject.SetActive(true);
                _vfxJump.Play();

                if (_soundJump != null)
                {
                    _soundJump.Play();
                }
            }

            if (_outlinable != null)
            {
                _outlinable.OutlineParameters.Color = _swimColor;
            }
           

            if (!_characterMovingModule.isMoving)
            {
                if (_vfxIdleSwim != null)
                {
                    _vfxIdleSwim.gameObject.SetActive(true);
                    _vfxIdleSwim.Play();
                }
            }
            else
            {
                _vfxSwim.gameObject.SetActive(true);
                _vfxSwim.Play();
            }

            _swim.Apply();
            _isSwim = true;
            onSwim.Dispatch();
            if (_swimTools != null)
            {
                _swimTools.SetActive(true);
            }
        }
        
        public void SetNotSwim()
        {
            _notSwim.Apply();
            _isSwim = false;
            
            if (_vfxIdleSwim != null)
            {
                _vfxIdleSwim.Stop();
            }
            
            if (_outlinable != null)
            {
                _outlinable.OutlineParameters.Color = _notSwimColor;
            }
            
            onNotSwim.Dispatch();
            if (_swimTools != null)
            {
                _swimTools.SetActive(false);
            }
            _vfxSwim.Stop();
        }

        private void FixedUpdate()
        {
            if(sea == null) return;

            if (_isJumpingToWater && CheckForStopJump())
            {
                StopJump();
            }
            
            if (_blockRaycast) return;

            if (isSwim && CheckForRaftFloor())
            {
                SetNotSwim();
            }else if (!isSwim && CheckForSea())
            {
                SetSwim();
            }

            if (!_isToAnimateStartEndSwim) return;
            CheckForStartSwim();
            CheckForSwimEnd();
        }

        public void BlockJumpIntoWater()
        {
            _isToAnimateStartEndSwim = false;
        }
        
        public void UnblockJumpIntoWater()
        {
            _isToAnimateStartEndSwim = true;
        }
        
        private bool CheckForStopJump()
        {
            Ray ray = new Ray(_pointForRaycastFloor.position, Vector3.up * -5f);
            RaycastHit hit;
            
            int layerMask = LayerMask.GetMask("GroundCollider", "SeaCollider");

            if (Physics.Raycast(ray, out hit, 5, layerMask))
            {
                if (hit.transform.GetComponent<Sea>() == null)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckForRaftFloor()
        {
            Ray ray = new Ray(_pointForRaycastFloor.position, Vector3.up * -5f);
            RaycastHit hit;
            
            int layerMask = LayerMask.GetMask("GroundCollider");

            if (Physics.Raycast(ray, out hit, 5, layerMask))
            {
                return true;
            }

            return false;
        }
        
        private bool CheckForSea()
        {
            return sea.seaCollider.bounds.Intersects(character.GetModule<SurroundInteractorCharacterSensorModule>().sensor
                .bounds);
        }

        private void CheckForStartSwim()
        {
            if (_blockRaycast) return;
            if (isSwim) return;
            if (_isJumpingToWater) return;

            Vector3 rayPos = _pointForRaycast.position;
            RaycastHit hit;
            
            int layerMask = LayerMask.GetMask("GroundCollider", "SeaCollider");

            if (Physics.SphereCast(rayPos, (character.bodyCollider as CapsuleCollider).radius / 2, transform.up * -1f, out hit, 2, layerMask))
            {
                if (hit.transform.GetComponent<Sea>() && _characterMovingModule.isMoving)
                {
                    rayPos += character.transform.forward * 1.5f;
                    Ray ray2 = new Ray(rayPos, Vector3.up * -5f);
                    RaycastHit hit2;

                    if (Physics.Raycast(ray2, out hit2, 5, layerMask))
                    {
                        if (hit2.transform.GetComponent<Sea>())
                        {
                            Vector3 vectorRight = (Quaternion.AngleAxis(-30, Vector3.up) * character.transform.forward) * 1.5f;
                            ray2 = new Ray(vectorRight, Vector3.up * -5f);
                            if (Physics.Raycast(ray2, out hit2, 5, layerMask))
                            {
                                if (!hit2.transform.GetComponent<Sea>())
                                {
                                    return;
                                }
                            }
                            Vector3 vectorLeft = (Quaternion.AngleAxis(30, Vector3.up) * character.transform.forward) * 1.5f;
                            ray2 = new Ray(vectorLeft, Vector3.up * -5f);
                            if (Physics.Raycast(ray2, out hit2, 5, layerMask))
                            {
                                if (!hit2.transform.GetComponent<Sea>())
                                {
                                    return;
                                }
                            }
                            
                            JumpToWater(hit.point);
                        }
                    }
                }
            }
        }
        
        private void CheckForSwimEnd()
        {
            if (_blockRaycast) return;
            if (!isSwim) return;
            
            Ray ray = new Ray(_pointForRaycastPlot.position, Vector3.up * -5f);
            RaycastHit hit;
            
            int layerMask = LayerMask.GetMask("GroundCollider");
 
            if (Physics.Raycast(ray, out hit, 5, layerMask))
            {
                if (!hit.transform.GetComponent<Sea>() && _characterMovingModule.isMoving)
                {
                    JumpToGround(hit.point);
                }
            }
        }

        private void JumpToWater(Vector3 hitPos)
        {
            DOVirtual.DelayedCall(0.05f, () =>
            {
                _isJumpingToWater = true;
            }, false).SetLink(gameObject);
          
            _blockRaycast = true;
            _jumpToWater.Apply();
            _jumpTween = character.transform.DOMoveY(0.4f, 0.325f).SetRelative(true).SetLink(gameObject);
            
            _jumpDelayedCall = DOVirtual.DelayedCall(0.95f, () =>
            {
                _isJumpingToWater = false;
                _blockRaycast = false;
            }, false).SetLink(gameObject);
        }

        private void StopJump()
        {
            _stopJumpToWater.Apply();
            
            if (_jumpTween != null)
            {
                _jumpTween.Kill();
            }
            if (_jumpDelayedCall != null)
            {
                _jumpDelayedCall.Kill();
            }
            
            _isJumpingToWater = false;
            
            DOVirtual.DelayedCall(0.1f, () =>
            {
                _blockRaycast = false;
            },false).SetLink(gameObject);
        }
        
        private void JumpToGround(Vector3 hitPos)
        {
            if (_swimTools != null)
            {
                _swimTools.SetActive(false);
            }
            _blockRaycast = true;
            _goingToGround = true;
            character.transform.DOMove(hitPos, 0.4f).SetLink(gameObject);
            _rb.isKinematic = true;

            DOVirtual.DelayedCall(0.4f, () =>
            {
                _rb.isKinematic = false;
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    _blockRaycast = false;
                    _goingToGround = false;
                },false).SetLink(gameObject);
            },false).SetLink(gameObject);
        }
    }
}