using GameCore.GameScene.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene.LevelItems.InteractItems
{
    public class FruitTreeSpotInteractItem : InteractItem
    {
        private FruitTreeItem _fruitTreeItemCached;
        
        public override int priority { get; } = 3;

        public FruitTreeItem fruitTreeItem
        {
            get
            {
                if (_fruitTreeItemCached == null) _fruitTreeItemCached = GetComponent<FruitTreeItem>();
                return _fruitTreeItemCached;
            }
        }
        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (!fruitTreeItem.isEnabled) return;
            fruitTreeItem.StartHitting(interactorModel);
        }
    }
}