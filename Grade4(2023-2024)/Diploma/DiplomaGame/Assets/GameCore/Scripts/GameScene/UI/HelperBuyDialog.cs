using GameCore.GameScene.Helper;
using JetBrains.Annotations;
using GameBasicsCore.Game.Models.GameCurrencies;
using GameBasicsCore.Game.Presenters;
using GameBasicsCore.Game.Presenters.Currencies;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using GameBasicsSignals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.GameScene.UI
{
    public class HelperBuyDialog : UIDialogWindow
    {
        [Inject, UsedImplicitly] public PlayerSoftCurrencyCollectModel softCurrencyCollectModel { get; }
        [InjectOptional, UsedImplicitly] public SoftCurrencyPresenter softCurrencyPresenter { get; }
        [Inject, UsedImplicitly] public BunchPopUpsMoveAnimationPresenter bunchPopUpsMoveAnimationPresenter { get; }
        
        [SerializeField] private HelperDataConfig _config;
        [SerializeField] private Button _btnBuy;
        [SerializeField] private TextMeshProUGUI _priceText;

        private bool _isBought;
        
        public readonly TheSignal onBuy = new();

        public override void Construct()
        {
            base.Construct();
            
            _priceText.text = _config.price + "";
            
            _btnBuy.onClick.AddListener(() =>
            {
                TryToBuy();
            });
            
            if (_config.price  > softCurrencyCollectModel.earned)
            {
                _btnBuy.interactable = false;
            }
        }

        private void TryToBuy()
        {
            if (_isBought) return;
            _isBought = true;
            if (_config.price <= softCurrencyCollectModel.earned)
            {
                softCurrencyCollectModel.Use(_config.price , "BuyHelper", "BuyHelper");
                onBuy.Dispatch();
                Hide();
            }
        }
    }
}