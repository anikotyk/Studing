using DG.Tweening;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Character.Modules;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Tweens;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Products
{
    public class WaterFilterObject : InjCoreMonoBehaviour
    {
        [SerializeField] private Transform _characterMovePoint;
        public Transform characterMovePoint => _characterMovePoint;
        [SerializeField] private Transform _characterReturnPoint;
        [SerializeField] private GameObject _wallsCollider;
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _vfx;
        [SerializeField] private WateringObject _wateringObject;
        [SerializeField] private Transform _camPoint;
        public Transform camPoint => _camPoint;
        [Inject, UsedImplicitly] public SignalHub hub { get; }
        
        private InteractItem _interactItemCached;
        public InteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<InteractItem>();
                return _interactItemCached;
            }
        }
        
        private bool _isEnabled = true;
        public bool isEnabled => _isEnabled;
        
        private bool _isNeedWater;
        public bool isNeedWater => _isNeedWater;

        private bool _isRunningNow;
        public bool isRunningNow => _isRunningNow;

        private InteractorCharacterView _lockerCharacterView;
        
        public readonly TheSignal onNeedsWater = new();
        public readonly TheSignal onNeedsWaterAndEnabled = new();
        public readonly TheSignal onRunningEnded = new();


        private void Awake()
        {
            _animator.enabled = false;
        }

        public override void Construct()
        {
            base.Construct();
            
            hub.Get<GCSgnl.WateringSignals.NeedsWater>().On((filter) =>
            {
                if (filter == this)
                {
                    OnNeedWater();
                }
            });
        }

        public void OnInteracted(InteractorCharacterView characterView, bool setPosInternal = false)
        {
            if(!CanInteract(characterView)) return;
            StartSceneOfWatering(characterView, setPosInternal);
            _isNeedWater = false;
        }

        public bool CanInteract(InteractorCharacterView characterView)
        {
            if(_lockerCharacterView!=null && _lockerCharacterView != characterView) return false;
            if(!characterView.GetModule<WateringModule>().CanInteract()) return false;
            return true;
        }

        public void StartSceneOfWatering(InteractorCharacterView characterView, bool setPosInternal = false)
        {
            interactItem.enabled = false;
            _isEnabled = false;
            _isRunningNow = true;

            if (!setPosInternal)
            {
                var mover = characterView.GetModule<MoveToStandOnPointCharacterModule>();
                mover.MoveAndLookInItsDirection(_characterMovePoint, 0.5f);
            }
            else
            {
                characterView.transform.position = _characterMovePoint.position;
                characterView.transform.rotation = _characterMovePoint.rotation;
            }
            
            characterView.GetModule<WateringModule>().onEndedWatering.Once(()=>
            {
                EndWatering(characterView, !setPosInternal);
            });
            characterView.GetModule<WateringModule>().onStartRun.On(()=>
            {
                _animator.enabled = true;
                _vfx.Play();
            });
            characterView.GetModule<WateringModule>().onStopRun.On(()=>
            {
                _animator.enabled = false;
                _vfx.Stop();
            });
            
            characterView.GetModule<WateringModule>().onCancelledWatering.On(()=>
            {
                CancelWatering(characterView);
            });
            
            
            characterView.GetModule<WateringModule>().StartWatering(this);

            DOVirtual.DelayedCall(0.1f, () =>
            {
                _wallsCollider.SetActive(true);
            },false).SetLink(gameObject);
        }

        public void EndWatering(InteractorCharacterView characterView, bool moveToReturnPoint = true)
        {
            if (moveToReturnPoint)
            {
                var mover = characterView.GetModule<MoveToStandOnPointCharacterModule>();
                mover.MoveAndLookInItsDirection(_characterReturnPoint, 0.5f); 
            }
            
            _isRunningNow = false;
            _animator.enabled = false;
            _vfx.Stop();
            
            _wallsCollider.SetActive(false);
            
            OnWateringEnd();
            
            DOVirtual.DelayedCall(GameplaySettings.def.wateringData.notAvailableTime, () =>
            {
                interactItem.enabled = true;
                _isEnabled = true;
                OnWaterFilterEnabled();
            },false).SetLink(gameObject);
            
            onRunningEnded.Dispatch();
        }
        
        public void CancelWatering(InteractorCharacterView characterView)
        {
            var mover = characterView.GetModule<MoveToStandOnPointCharacterModule>();
            mover.MoveAndLookInItsDirection(_characterReturnPoint, 0.5f);
            
            _isRunningNow = false;
            _animator.enabled = false;
            _vfx.Stop();
            
            _wallsCollider.SetActive(false);

            DOVirtual.DelayedCall(GameplaySettings.def.wateringData.notAvailableTime, () =>
            {
                interactItem.enabled = true;
                _isEnabled = true;
                OnWaterFilterEnabled();
            },false).SetLink(gameObject);
            
            onRunningEnded.Dispatch();
        }
        
        private void OnWaterFilterEnabled()
        {
            if(_isNeedWater)
            {
                onNeedsWaterAndEnabled.Dispatch();
            }
        }
        
        private void OnWateringEnd()
        {
            _isNeedWater = false;
            _wateringObject.ShowWatering();
        }
        
        private void OnNeedWater()
        {
            _isNeedWater = true;
            
            onNeedsWater.Dispatch();
            if (isEnabled)
            {
                onNeedsWaterAndEnabled.Dispatch();
            }
        }

        public void LockInteractions(InteractorCharacterView characterView)
        {
            _lockerCharacterView = characterView;
        }
        
        public void UnlockInteractions()
        {
            _lockerCharacterView = null;
        }
    }
}