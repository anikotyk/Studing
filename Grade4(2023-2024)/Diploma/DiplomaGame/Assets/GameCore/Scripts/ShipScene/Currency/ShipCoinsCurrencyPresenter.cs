using System;
using JetBrains.Annotations;
using GameBasicsCore.Game.Models.GameCurrencies;
using GameBasicsCore.Game.Presenters.Currencies;
using GameBasicsCore.Game.Views.UI.PopUps.GameCurrency;
using Zenject;

namespace GameCore.ShipScene.Currency
{
    public class ShipCoinsCurrencyPresenter : GameCurrencyPresenter
    {
        [Inject, UsedImplicitly] public ShipCoinsCollectModel shipCoinsCollectModel { get; }
        public override GameCurrencyCollectModel collectModel => shipCoinsCollectModel;
        
        protected override (Type type, string id) GetPopUpData()
        {
            return (typeof(SoftCurrencyFlyingPopUpView), null);
        }
    }
}