using GameCore.GameScene_Island.Controllers.ObjectContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;

namespace GameCore.GameScene_Island.LevelItems.InteractItems
{
    public class SellProductsCollectPlatformInteractItem : CollectingModuleInteractItem<SellProductsCollectingController>
    {
        public override int priority { get; } = 3;
    }
}