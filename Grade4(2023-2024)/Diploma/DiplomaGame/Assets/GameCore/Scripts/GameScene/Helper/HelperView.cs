using GameCore.GameScene_Iceland.LevelItems.Helper.Modules;
using GameCore.GameScene.Helper.Modules;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;

namespace GameCore.GameScene.Helper
{
    public class HelperView : InteractorCharacterView
    {
        private HelperAnimationsModule _animationsModuleCached;
        public HelperAnimationsModule animationsModule => _animationsModuleCached ??=
            GetComponentInChildren<HelperAnimationsModule>(true);

        private HelperLogicModule _logicModuleCached;
        public HelperLogicModule logicModule => _logicModuleCached ??=
            GetComponentInChildren<HelperLogicModule>(true);

        private HelperTaskModule _taskModuleCached;
        public HelperTaskModule taskModule => _taskModuleCached ??= GetComponentInChildren<HelperTaskModule>(true);

        private LocomotionHelperMovingModule _locomotionMovingModuleCached;
        public LocomotionHelperMovingModule locomotionMovingModule => _locomotionMovingModuleCached ??= GetComponentInChildren<LocomotionHelperMovingModule>(true);

        private HelperSpeedModule _speedModuleCached;
        public HelperSpeedModule speedModule => _speedModuleCached ??= GetComponentInChildren<HelperSpeedModule>(true);

        private HelperInteractorEnoughProductsModule _enoughProductsModuleCached;
        public HelperInteractorEnoughProductsModule enoughProductsModule => _enoughProductsModuleCached ??= GetComponentInChildren<HelperInteractorEnoughProductsModule>(true);

        private HelperSellStorageModule _sellStorageModuleCached;
        public HelperSellStorageModule sellStorageModule => _sellStorageModuleCached ??= GetComponentInChildren<HelperSellStorageModule>(true);

    }
}