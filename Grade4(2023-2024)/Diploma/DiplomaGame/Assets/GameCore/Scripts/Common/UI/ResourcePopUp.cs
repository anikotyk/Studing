using GameBasicsCore.Game.Views.UI.PopUps;

namespace GameCore.Common.UI
{
    public class ResourcePopUp : ImagePopUpView
    {
        private void OnEnable()
        {
            var seq = FlyUpAnimation(200f);
        }
    }
}