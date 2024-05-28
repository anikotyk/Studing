using GameCore.GameScene.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.Models;
using GameBasicsSignals;

namespace GameCore.Common.Misc
{
    /// <summary>
    /// Global Signals of Game Core that go through main SignalHub
    /// </summary>
    public static class GCSgnl
    {
        public static class SellSignals
        {
            public class Interact : ASignal<InteractorCharacterModel, SellProduct> {}
        }
        public static class WateringSignals
        {
            public class NeedsWater : ASignal<WaterFilterObject> {}
        }

        public static class SharkSignals
        {
            public class Interact : ASignal<InteractorCharacterModel, MeatAnimalItem> {}
        }

        public static class SwimHuntingSignals
        {
            public class Started : ASignal {}
            public class Ended : ASignal {}
        }
    }
    
}