using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene.Helper;

namespace GameCore.GameScene_Island.LevelItems.Helper.Modules
{
    public class HelperCuttingModule : CuttingModule
    {
       private float _maxSpeed;
        
        private HelperView _viewCached;
        public HelperView view => _viewCached ??= GetComponentInParent<HelperView>();

        protected override void SetSpeedCutting()
        {
            _maxSpeed = view.taskModule.aiPath.maxSpeed;
            view.taskModule.aiPath.maxSpeed = moveSpeed;
        }
        
        protected override void SetSpeedEndCutting()
        {
            view.taskModule.aiPath.maxSpeed = _maxSpeed;
        }
    }
}