using GameCore.ShipScene.Common;
using GameBasicsCore.Game.Models.GameCurrencies;

namespace GameCore.ShipScene.Currency
{
    public class ShipCoinsCollectModel : GameCurrencyCollectModel
    {
        public override string name => settings.currencyName;
        
        public ShipCurrencyModule settings => ShipSceneSettings.def.currencyModule;

        public ShipCoinsCollectModel()
        {
            SetupEarned(ShipSceneConstants.shipCoinsSaveKey, settings.startAmount.value);
        }
    }
}