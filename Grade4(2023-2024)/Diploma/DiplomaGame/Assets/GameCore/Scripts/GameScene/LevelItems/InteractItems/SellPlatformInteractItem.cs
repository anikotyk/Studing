using GameCore.GameScene.Controllers.ObjectContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;

namespace GameCore.GameScene.LevelItems.InteractItems
{
    public class SellPlatformInteractItem : CollectingModuleInteractItem<SellCollectingController>
    {
        public override int priority { get; } = 3;
    }
}