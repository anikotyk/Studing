using DG.Tweening;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Misc
{
    public class CarrierCountDisplayer : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        
        [SerializeField] private ProductsCarrier _productsCarrier;
        
        [SerializeField] private Transform _popUpPoint;

        private TextPopUpView _spacePopUp;
        private bool _isPopUpShown;
        private Tween _popUpTween;
        private Tween _popUpHideTween;

        public override void Construct()
        {
            base.Construct();
            
            SetUpPopUp();
        }

        private void SetUpPopUp()
        { 
            _spacePopUp = popUpsController.SpawnUnderMenu<TextPopUpView>("SpacePopUp");
            _spacePopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
            _spacePopUp.worldSpaceConverter.followWorldObject = _popUpPoint;
            _spacePopUp.transform.localScale = Vector3.zero;
            _spacePopUp.SetText("0/"+_productsCarrier.capacity);

            _productsCarrier.onAddComplete.On((_, _) =>
            {
                _spacePopUp.SetText(_productsCarrier.count + "/" + _productsCarrier.capacity);
                if (!_productsCarrier.HasSpace())
                {
                    _spacePopUp.SetText("MAX");
                }

                if (!_isPopUpShown)
                {
                    ShowPopUp();
                }

                if (_popUpHideTween != null)
                {
                    _popUpHideTween.Kill();
                }
                _popUpHideTween = DOVirtual.DelayedCall(2f, () =>
                {
                    HidePopUp();
                }).SetLink(gameObject);
            });
        }
        
        private void HidePopUp()
        {
            _isPopUpShown = false;
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            _spacePopUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }
        
        private void ShowPopUp()
        {
            _isPopUpShown = true;
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            _spacePopUp.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }
    }
}