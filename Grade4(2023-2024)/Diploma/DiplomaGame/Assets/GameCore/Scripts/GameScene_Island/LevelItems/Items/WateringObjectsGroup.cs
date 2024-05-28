using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Items;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Misc;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class WateringObjectsGroup : InjCoreMonoBehaviour, IWatering
    {
        [Inject, UsedImplicitly] public SignalHub hub { get;} 
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        
        [SerializeField] private Transform _unwateredPopUpPoint;
        [SerializeField] private GameObject _groundUnwatered;
        [SerializeField] private GameObject _groundWatered;
        [SerializeField] private WaterFilterObject _waterFilterObject;

        public Transform pointWatering => transform;
        
        private ImagePopUpView _unwateredPopUp;
        
        private bool _isUnwatered;
        private int _cntUnwatered;
        
        private BuyObject _buyObjectCached;

        public BuyObject buyObject
        {
            get
            {
                if (_buyObjectCached == null) _buyObjectCached = GetComponentInParent<BuyObject>(true);
                return _buyObjectCached;
            }
        }
        
        private Tween _popUpTween;

        private WateringCuttableItem[] _itemsCached;

        public WateringCuttableItem[] items
        {
            get
            {
                if (_itemsCached == null)
                    _itemsCached = GetComponentsInChildren<WateringCuttableItem>(true);
                return _itemsCached;
            }
        }

        public override void Construct()
        {
            base.Construct();

            foreach (var item in items)
            {
                item.onReadyToRespawn.On(() =>
                {
                    _cntUnwatered++;
                    if (_cntUnwatered >= items.Length)
                    {
                        SetUnwatered();
                    }
                });
            }
        }

        public void OnWatered()
        {
            if (buyObject.isBought)
            {
                _cntUnwatered = 0;
                _isUnwatered = false;
                _groundUnwatered.SetActive(false);
                _groundWatered.SetActive(true);
                HideUnwateredPopUp();

                foreach (var item in items)
                {
                    item.OnWatered();
                }
            }
        }

        private void SetUnwatered()
        {
            if (_isUnwatered) return;
            _isUnwatered = true;
            
            _groundUnwatered.SetActive(true);
            _groundWatered.SetActive(false);
            
            hub.Get<GCSgnl.WateringSignals.NeedsWater>().Dispatch(_waterFilterObject);

            SetupUnwateredPopUp();
        }
        
        public void SetWateredAndRespawned()
        {
            _cntUnwatered = 0;
            _isUnwatered = false;
            _groundUnwatered.SetActive(false);
            _groundWatered.SetActive(true);
            HideUnwateredPopUp();
            
            foreach (var item in items)
            {
                item.OnWateredAndRespawned();
            }
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