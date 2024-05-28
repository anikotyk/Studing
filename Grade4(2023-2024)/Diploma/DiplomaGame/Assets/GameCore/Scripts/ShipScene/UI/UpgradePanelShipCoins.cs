using GameCore.GameScene.UI;
using GameCore.ShipScene.Currency;
using JetBrains.Annotations;
using GameBasicsCore.Game.Models.GameCurrencies;
using Zenject;

namespace GameCore.ShipScene.UI
{
    public class UpgradePanelShipCoins : UpgradePanel
    {
        [Inject, UsedImplicitly] public ShipCoinsCollectModel shipCoinsCollectModel { get; }
        protected override GameCurrencyCollectModel gameCurrencyCollectModel => shipCoinsCollectModel;
    }
}