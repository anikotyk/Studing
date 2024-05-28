using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.ShipScene.Common;
using GameCore.ShipScene.Currency;
using JetBrains.Annotations;
using NaughtyAttributes;
using GameBasicsCore.Game.API.Analytics;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSignals;
using StaserSDK.Interactable;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Interactions
{
    public class CoinsBuyZone : InjCheckCoreMonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private ZoneBase _zoneBase;
        [SerializeField] private int _price;
        [SerializeField] private BuyZoneAnimation _coinsBuyZoneAnimation;
        [Header("PopUp")]
        [SerializeField] private Transform _popUpPoint;
        [SerializeField] private bool _showLabel;
        [SerializeField, ShowIf(nameof(_showLabel))] private string _label;
        
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public ShipCoinsCollectModel shipCoinsCollectModel { get; }
        [Inject, UsedImplicitly] public LevelEventsManager levelEventsManager { get; }

        private int _cachedProgress = 0;
        private bool _sendedEvent = false;

        public ZoneBase zone => _zoneBase;
        
        private ShipCoinsBuyPopUp _buyPopUpCached;
        public ShipCoinsBuyPopUp buyPopUp
        {
            get
            {
                if (_buyPopUpCached == null)
                    InitBuyPopUp();
                return _buyPopUpCached;
            }
        }

        private TheSaveProperty<int> _buyProgressCached;
        public TheSaveProperty<int> buyProgress =>
            _buyProgressCached ??= new TheSaveProperty<int>(_id, domain:ShipSceneConstants.saveFile);

        public TheSignal<int> used { get; } = new();
        public TheSignal bought { get; } = new();
        private void OnEnable()
        {
            _zoneBase.onInteract.On(OnInteract);
            _coinsBuyZoneAnimation.added.On(OnAdded);
            if (IsInjected && IsBought() == false)
                ShowBuyPopUp();
        }

        private void OnDisable()
        {
            _zoneBase.onInteract.Off(OnInteract);
            _coinsBuyZoneAnimation.added.Off(OnAdded);
            if (IsInjected && !_isDestroyed && _buyPopUpCached)
                HideBuyPopUp();
        }
        
        private bool _isDestroyed = false;
        protected override void OnDestroy()
        {
            _isDestroyed = true;
            base.OnDestroy();
        }

        public override void Construct()
        {
            base.Construct();
            InitBuyPopUp();
        }

        private void OnAdded(int addedValue)
        {
            buyProgress.value = Mathf.Min(buyProgress.value + addedValue, _price);
            _cachedProgress = Mathf.Max(0, _cachedProgress - addedValue);
            Validate(true);
            if (IsBought() && _sendedEvent == false)
            {
                _sendedEvent = true;
                levelEventsManager.OnLevelStepState(LevelEventsManager.EventState.Complete, "Bought"+_id);
            }
        }

        private void OnInteract(InteractableCharacter character)
        {
            if(_cachedProgress > 0 || IsBought())
                return;
            
            int playerCoins = System.Convert.ToInt32(shipCoinsCollectModel.earned);
            if(playerCoins == 0)
                return;
            
            int coinsLeft = _price - buyProgress.value;
            int coinsToTake = Mathf.Clamp(playerCoins, 0, coinsLeft);

            _cachedProgress += coinsToTake;
            var resource = shipCoinsCollectModel.settings.name;
            shipCoinsCollectModel.Use(coinsToTake,resource, resource);
            used.Dispatch(coinsToTake);
        }

        public bool IsBought()
        {
            return buyProgress.value >= _price || buyProgress.value + _cachedProgress >= _price;
        }
        
        private void Validate(bool animate = false)
        {
            buyPopUp.SetPrice(_price - buyProgress.value);
            if(animate)
                buyPopUp.Punch();
            if (IsBought())
                Buy();
        }

        private void Buy()
        {
            bought.Dispatch();
            HideBuyPopUp().OnComplete(() =>
            {
                buyPopUp.Complete();
            });
        }

        [Button()]
        private void Debug()
        {
            used.Dispatch(5);
        }

        private void InitBuyPopUp()
        {
            if (_buyPopUpCached != null)
                return;
            _buyPopUpCached = popUpsController.SpawnUnderMenu<ShipCoinsBuyPopUp>();
            _buyPopUpCached.worldSpaceConverter.updateMethod = UpdateMethod.Update;
            _buyPopUpCached.worldSpaceConverter.followWorldObject = _popUpPoint;
            _buyPopUpCached.transform.localScale = Vector3.zero;
            if(_showLabel == false)
                _buyPopUpCached.HideLabel();
            else
                _buyPopUpCached.ShowLabel(_label);
            if (gameObject.activeInHierarchy)
                ShowBuyPopUp();
            Validate();
        }

        public Tween ShowBuyPopUp()
        {
            DOTween.Kill(buyPopUp);
            return buyPopUp.transform.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.OutBack).SetLink(gameObject).SetId(buyPopUp);
        }
        
        public Tween HideBuyPopUp()
        {
            DOTween.Kill(buyPopUp);
            return buyPopUp.transform.DOScale(Vector3.zero, 0.5f)
                .SetEase(Ease.InBack).SetLink(gameObject).SetId(buyPopUp);
        }
        
    }
}