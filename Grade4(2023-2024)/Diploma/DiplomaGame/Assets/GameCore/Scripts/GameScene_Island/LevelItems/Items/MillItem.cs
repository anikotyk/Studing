using DG.Tweening;
using GameCore.Common.Misc;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene.Audio;
using GameCore.GameScene.Misc;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Tweens;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class MillItem : InjCoreMonoBehaviour
    {
        [SerializeField] private InteractableProductionItem _interactableProductionItem;
        public InteractableProductionItem interactableProductionItem => _interactableProductionItem;
        [SerializeField] private Transform _interactPoint;
        public Transform interactPoint => _interactPoint;
        
        [SerializeField] private Transform _rotator;
        [SerializeField] private Transform _characterFollowPoint;
        
        [SerializeField] private Transform _camPoint;
        public Transform camPoint => _camPoint;
        
        [SerializeField] private Transform _spotVisual;
        [Inject, UsedImplicitly] public SignalHub hub { get; }
        [Inject, UsedImplicitly] public StallSoundManager stallSoundManager { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
         
        private InteractItem _interactItemCached;
        public InteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<InteractItem>();
                return _interactItemCached;
            }
        }
        
        private bool _isGrinding;
        
        private bool _isEnabled = true;
        public bool isEnabled => _isEnabled;

        private bool _isSpotVisible;
        private Tween _spotTween;
        
        public readonly TheSignal onEnabled = new();
        public readonly TheSignal onEndMill = new();

        public override void Construct()
        {
            base.Construct();

            _spotVisual.gameObject.SetActive(false);
            _interactableProductionItem.onCanStartProduct.On(ShowSpot);
        }

        public void OnInteracted(InteractorCharacterModel characterModel)
        {
            if(!characterModel.view.GetModule<MillGrindingModule>().CanInteract()) return;
            StartGrinding(characterModel);
        }

        public void StartGrinding(InteractorCharacterModel characterModel)
        {
            if(_isGrinding) return;
            _isGrinding = true;
            
            interactItem.enabled = false;
            _isEnabled = false;
            HideSpot();
            _interactableProductionItem.StartWorking();

            var mover = characterModel.view.GetModule<MoveToStandOnPointCharacterModule>();
            mover.MoveAndLookInItsDirection(_characterFollowPoint, 0.5f);
            
            characterModel.view.GetModule<MillGrindingModule>().onMove.On((coef)=>
            {
                MoveRotatorAndCharacter(coef, characterModel);
            });
            
            characterModel.view.GetModule<MillGrindingModule>().StartGrinding(this);
        }
        
        private void MoveRotatorAndCharacter(float coef ,InteractorCharacterModel characterModel)
        {
            if(!_isGrinding) return;
            _rotator.localRotation = Quaternion.Euler(new Vector3(0, coef * -360, 0));
            characterModel.view.transform.position = _characterFollowPoint.position;
            characterModel.view.transform.forward = _characterFollowPoint.forward;

            if (coef >= 1)
            {
                EndGrinding(characterModel);
            }
        }

        public void EndGrinding(InteractorCharacterModel characterModel)
        {
            if(!_isGrinding) return;
            _isGrinding = false;
            
            _rotator.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            
            characterModel.view.GetModule<MillGrindingModule>().EndGrinding();
            
            _interactableProductionItem.EndWorking();
           
            DOVirtual.DelayedCall(0.5f, () =>
            {
                interactItem.enabled = true;
                _isEnabled = true;
            },false).SetLink(gameObject);
        }

        public void ShowSpot()
        {
            if(_isSpotVisible) return;
            _isSpotVisible = true;
            
            if (_spotTween != null)
            {
                _spotTween.Kill();
            }

            _spotVisual.gameObject.SetActive(true);
            _spotTween = _spotVisual.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                interactItem.enabled = true;
            }).SetLink(gameObject);
        }
        
        public void HideSpot()
        {
            if(!_isSpotVisible) return;
            _isSpotVisible = false;
            if (_spotTween != null)
            {
                _spotTween.Kill();
            }

            interactItem.enabled = false;
            _spotTween = _spotVisual.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _spotVisual.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }
    }
}