using NaughtyAttributes;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Settings.GameCore.Modules.GameCurrency;
using GameBasicsCore.Game.Views.UI.Controls.Currencies.Displays;
using Zenject;

namespace GameCore.ShipScene.Currency
{
    public class ShipCoinsDisplay : GameCurrencyDisplay<ShipCoinsCurrencyPresenter>
    {
        [Inject] private ShipCoinsCollectModel _shipCoinsCollectModel;
        protected override GameCurrencyGCSModule _settings => ShipSceneSettings.def.currencyModule;
        
        protected override void DispatchNotEnough()
        {
            hub.Get<NCSgnl.GameCurrencySignals.NoEnoughSoft>().Dispatch();
        }

        [Button()]
        private void Earn()
        {
            _shipCoinsCollectModel.Earn(5.0, _settings.currencyName, _settings.currencyName);
        }
        
        [Button()]
        private void Collect()
        {
            _shipCoinsCollectModel.Collect(5.0, _settings.currencyName, _settings.currencyName);
        }
    }
}