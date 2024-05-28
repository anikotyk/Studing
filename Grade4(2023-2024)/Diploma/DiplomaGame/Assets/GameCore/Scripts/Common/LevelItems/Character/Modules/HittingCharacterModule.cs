using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore.Common.LevelItems.Items.HittableItems;
using GameCore.Common.Misc;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Sensors;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.Common.LevelItems.Character.Modules
{
    public class HittingCharacterModule : InteractorCharacterModule
    {
        [SerializeField] private List<ToolView> _toolsViews;
        private float _hitMultipler = 1;
        private float _distanceIgnoreAngle = 0.5f;
        public InteractionCharacterModule interactionModule => character.GetModule<InteractionCharacterModule>();

        private List<IHittable> _hittableItems = new List<IHittable>();

        private SphereCollider _characterSensor;
        private Coroutine _hittingCoroutine;

        private bool _isSuperPower;
        
        private bool _isHitting;
        public override bool isNowRunning => _isHitting;

        private ToolView _currentTool;

        public readonly TheSignal onHitting = new();
        public readonly TheSignal onNotHitting = new();

        public void SetHitMultiplier(float multiplier)
        {
            _hitMultipler = multiplier;
        }

        public void OnSuperPower()
        {
            _isSuperPower = true;
        }
        
        public void OnEndSuperPower()
        {
            _isSuperPower = false;
        }
        
        public void ResetHitMultiplier()
        {
            _hitMultipler = 1;
        }

        public bool CanInteract(CharacterTools.HittingToolType toolType)
        {
            return _toolsViews.FirstOrDefault(item => item.type == toolType) != null;
        }

        public override void Construct()
        {
            base.Construct();
            CharacterMovingModule movingModule = character.GetModule<CharacterMovingModule>();

            _characterSensor = character.GetModule<SurroundInteractorCharacterSensorModule>().sensor;
            movingModule.onStartMoving.On(() =>
            {
                StopHitting();
            });
            movingModule.onStopMoving.On(() =>
            {
                if (_hittableItems.Count > 0)
                {
                    if (IsAnyItemInHittingView())
                    {
                        StartHitting();
                    }
                }
            });
        }

        public void EnteredHittableItem(IHittable item)
        {
            if(_hittableItems.Contains(item)) return;
            if(!CanInteract(item.toolType)) return;
            
            _hittableItems.Add(item);

            item.onTurnOff.Once(() =>
            {
                ExitedHittableItem(item);
            });

            if (!_isHitting && !character.GetModule<CharacterMovingModule>().isMoving && IsAnyItemInHittingView())
            {
                StartHitting();
            }
        }
        
        public void ExitedHittableItem(IHittable item)
        {
            if(!_hittableItems.Contains(item)) return;
            _hittableItems.Remove(item);

            if (_hittableItems.Count <= 0 || IsAnyItemInHittingView())
            {
                StopHitting();
            }
        }

        public void StopHitting()
        {
            if (!_isHitting) return;
            _isHitting = false;
            
            if (_hittingCoroutine != null)
            {
                StopCoroutine(_hittingCoroutine);
                _hittingCoroutine = null;
            }

            if (_currentTool != null)
            {
                _currentTool.DeactivateView();
                _currentTool.hitEndApplier.Apply();
            }
            
            interactionModule.OnInteractionEnd(this);
            
            onNotHitting.Dispatch();
        }
        
        public void StartHitting()
        {
            if (!interactionModule.CanInteract()) return;
            if (_isHitting) return;
            _isHitting = true;

            _currentTool = GetCurrentTool();
            if(_currentTool!= null) _currentTool.ActivateView(_isSuperPower);
            
            interactionModule.OnInteractionStart(this);
            
            onHitting.Dispatch();
            
            _hittingCoroutine = StartCoroutine(HitItemsCoroutine());
        }

        private IEnumerator HitItemsCoroutine()
        {
            while (true)
            {
                if (!IsAnyItemInHittingView())
                {
                    break;
                }
                while (character.animator.GetCurrentAnimatorStateInfo(_currentTool.animationLayer).IsName(_currentTool.animationName)) yield return null;
                
                _currentTool.hitApplier.Apply();
                
                yield return new WaitForSeconds(0.4f);
                
                _currentTool.toolVfx.Play();
                if (_currentTool.hitSound != null) _currentTool.hitSound.Play();
                
               
                foreach (var item in _hittableItems.ToArray())
                {
                    if (item.isEnabled && IsIntersectItemCollider(item))
                    {
                        item.OnHit(_currentTool.defaultMultiplier * _hitMultipler);
                    }
                }
            }
            
            StopHitting();
        }

        private bool IsIntersectItemCollider(IHittable hittable)
        {
            return hittable.colliderInteract.bounds.Intersects(_characterSensor.bounds);
        }
        
        private bool IsAnyItemInHittingView()
        {
            return _hittableItems.FirstOrDefault((item) =>
            {
                return IsIntersectItemCollider(item) && (IsItemInCorrectAngle(item) || IsItemInShortDistance(item));
            }) != null;
        }

        private bool IsItemInCorrectAngle(IHittable item)
        {
            return Vector3.Angle(character.transform.forward, item.view.position - character.transform.position) <= item.canHitAngle;
        }
        
        private bool IsItemInShortDistance(IHittable item)
        {
            return Vector3.Distance(character.transform.position, item.view.position) <= _distanceIgnoreAngle;
        }

        private ToolView GetCurrentTool()
        {
            var toolCounts = _hittableItems.GroupBy(h => h.toolType).ToDictionary(g => g.Key, g => g.Count());
            var mostUsableTool = toolCounts.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;
            var toolView = _toolsViews.FirstOrDefault(toolView => toolView.type == mostUsableTool);
            
            return toolView;
        }
    }

    [Serializable]
    [CanBeNull]
    public class ToolView
    {
        public CharacterTools.HittingToolType type;
        public GameObject view;
        public GameObject viewSuperPower;
        public ParticleSystem toolVfx;
        public AnimatorParameterApplier hitApplier;
        public AnimatorParameterApplier hitEndApplier;
        public string animationName;
        public int animationLayer;
        public float defaultMultiplier;
        public AudioSource hitSound;

        private GameObject GetView(bool isSuperPower)
        {
            return isSuperPower && viewSuperPower ? viewSuperPower : view;
        }

        public void DeactivateView()
        {
            view.SetActive(false);
            if(viewSuperPower) viewSuperPower.SetActive(false);
        }
        
        public void ActivateView(bool isSuperPower)
        {
            GetView(isSuperPower).SetActive(true);
        }
    }
}