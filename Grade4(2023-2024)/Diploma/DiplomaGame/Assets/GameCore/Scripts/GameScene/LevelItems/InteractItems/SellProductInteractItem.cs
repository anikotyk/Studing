using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene.LevelItems.InteractItems
{
    public class SellProductInteractItem : InteractItem
    {
        public override int priority { get; } = 4;
        
        private SellProduct _sellProductCached;

        public SellProduct sellProduct
        {
            get
            {
                if (_sellProductCached == null) _sellProductCached = GetComponent<SellProduct>();
                return _sellProductCached;
            }
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            sellProduct.hub.Get<GCSgnl.SellSignals.Interact>().Dispatch(interactorModel, sellProduct);
        }
    }
}