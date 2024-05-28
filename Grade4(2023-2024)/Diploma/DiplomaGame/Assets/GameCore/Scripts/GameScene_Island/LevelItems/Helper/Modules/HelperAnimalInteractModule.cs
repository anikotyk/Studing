using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene.Helper;

namespace GameCore.GameScene_Island.LevelItems.Helper.Modules
{
    public class HelperAnimalInteractModule : AnimalInteractModule
    {
        private HelperView _viewCached;
        public HelperView view => _viewCached ??= GetComponentInParent<HelperView>();
        
        protected override void DisableMovement()
        {
            view.taskModule.aiPath.canMove = false;
        }
        
        protected override void EnableMovement()
        {
            view.taskModule.aiPath.canMove = true;
        }
    }
}