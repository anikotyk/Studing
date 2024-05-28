using DG.Tweening;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Items.HittableItems
{
    public class HittableWateringItem : HittableRespawningItem, IWatering
    {
        [Inject, UsedImplicitly] public SignalHub hub { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        
        [SerializeField] private WaterFilterObject _waterFilterObject;
        [SerializeField] private Transform _unwateredPopUpPoint;
        [SerializeField] private GameObject _groundUnwatered;
        [SerializeField] private GameObject _groundWatered;
        
        public Transform pointWatering => transform;
        
        private bool _isUnwatered = false;
        private ImagePopUpView _unwateredPopUp;
        private Tween _popUpTween;
        
        protected override void OnStagesEnded()
        {
            SetInUnwateredMode();
        }

        public void OnWatered()
        {
            if (_isUnwatered)
            {
                StartRespawn();
                HideUnwateredPopUp();
                if (_groundUnwatered != null)
                {
                    _groundUnwatered.SetActive(false);
                }
                if (_groundWatered != null)
                {
                    _groundWatered.SetActive(true);
                }
            }
            _isUnwatered = false;
        }
        
        private void SetInUnwateredMode()
        {
            if (_groundUnwatered != null)
            {
                _groundUnwatered.SetActive(true);
            }
            if (_groundWatered != null)
            {
                _groundWatered.SetActive(false);
            }
            _isUnwatered = true;
            SetupUnwateredPopUp();
            onStartedRespawn.Dispatch();
            hub.Get<GCSgnl.WateringSignals.NeedsWater>().Dispatch(_waterFilterObject);
        }
        
        private void SetupUnwateredPopUp()
        {
            if (_unwateredPopUp == null)
            {
                _unwateredPopUp = popUpsController.SpawnUnderMenu<ImagePopUpView>("Unwatered");
                _unwateredPopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
                _unwateredPopUp.worldSpaceConverter.followWorldObject = _unwateredPopUpPoint.transform;
            }
           
            _unwateredPopUp.transform.localScale = Vector3.zero;
            _unwateredPopUp.gameObject.SetActive(true);
            
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            _popUpTween = _unwateredPopUp.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }

        private void HideUnwateredPopUp()
        {
            if (_unwateredPopUp == null) return;
            
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            
            var popUp = _unwateredPopUp;
            _popUpTween = popUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                popUp.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }
    }
}