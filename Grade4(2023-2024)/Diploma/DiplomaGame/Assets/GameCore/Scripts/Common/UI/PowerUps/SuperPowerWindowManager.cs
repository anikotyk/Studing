using GameCore.Common.Misc;

namespace GameCore.Common.UI.PowerUps
{
    public class SuperPowerWindowManager : PowerUpWindowManager
    {
        public SuperPowerDialog superPowerDialog => (SuperPowerDialog) dialog;
        protected override void CreateDialog()
        {
            uiWindowFactory.Create<SuperPowerDialog>(CommStr.UIWindowSuperPower, canvasesRef.bottomOverlapCanvas .transform, InitAndShowWindow);
        }
    }
}
