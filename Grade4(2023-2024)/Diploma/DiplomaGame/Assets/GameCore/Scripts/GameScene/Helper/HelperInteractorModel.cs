using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene.Helper
{
    public class HelperInteractorModel : InteractorCharacterModel
    {
        public void DisableMovement(object id)
        {
            BlockInteractions(id);
        }

        public void EnableMovement(object id)
        {
            UnblockInteractions(id);
        }
    }
}