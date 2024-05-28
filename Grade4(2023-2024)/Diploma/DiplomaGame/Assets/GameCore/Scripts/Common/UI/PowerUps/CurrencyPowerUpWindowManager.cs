using GameCore.Common.Misc;

namespace GameCore.Common.UI.PowerUps
{
    public class CurrencyPowerUpWindowManager : PowerUpWindowManager
    {
        public CurrencyPowerUpDialog currencyPowerUpDialog => (CurrencyPowerUpDialog) dialog;
        protected override void CreateDialog()
        {
            uiWindowFactory.Create<CurrencyPowerUpDialog>(CommStr.CurrencyPowerUpDialog, canvasesRef.bottomOverlapCanvas .transform, InitAndShowWindow);
        }
    }
}
