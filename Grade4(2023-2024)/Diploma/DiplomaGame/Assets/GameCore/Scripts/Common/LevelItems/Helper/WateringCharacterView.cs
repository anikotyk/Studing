using GameCore.Common.LevelItems.Helper.Modules;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;

namespace GameCore.Common.LevelItems.Helper
{
    public class WateringCharacterView : InteractorCharacterView
    {
        private AutoWateringLogicModule _autoWateringLogicModuleCached;
        public AutoWateringLogicModule autoWateringLogicModule => _autoWateringLogicModuleCached ??=
            GetComponentInChildren<AutoWateringLogicModule>(true);
    }
}