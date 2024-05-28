using DG.Tweening;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.GameScene.LevelItems;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.Common.LevelItems.InteractItems
{
    public class FishingPlatformInteractItem : InteractItem
    {
        [SerializeField] private Transform _characterPoint;
        [SerializeField] private FishingRod _fishingRod;
        [SerializeField] private Transform _platformVisible;
        public override int priority { get; } = 5;

        private bool _isActive = true;
        private Tween _scaleTween;

        public override void Construct()
        {
            base.Construct();
            _fishingRod.carrier.onChange.On(() =>
            {
                if (!_fishingRod.carrier.HasSpace())
                {
                    Deactivate();
                }
                else
                {
                    Activate();
                }
            });
        }

        public override bool CanInteract(InteractorCharacterModel interactorModel)
        {
            return interactorModel.view.GetModule<FishingCharacterModule>() && _isActive;
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if(!_isActive) return;
            interactorModel.view.GetModule<FishingCharacterModule>().StartFishing(_characterPoint, _fishingRod);
        }

        private void Deactivate()
        {
            if(!_isActive) return;
            _isActive = false;
            if (_scaleTween != null) _scaleTween.Kill();
            _scaleTween = _platformVisible.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }
        
        private void Activate()
        {
            if(_isActive) return;
            _isActive = true;
            if (_scaleTween != null) _scaleTween.Kill();
            _scaleTween = _platformVisible.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }
    }
}