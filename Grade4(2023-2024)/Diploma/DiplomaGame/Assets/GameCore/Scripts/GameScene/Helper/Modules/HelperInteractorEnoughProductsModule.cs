using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Capacity;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using Zenject;

namespace GameCore.GameScene.Helper.Modules
{
    public class HelperInteractorEnoughProductsModule : InteractorEnoughProductsModule
    {
        private InteractorEnoughProductsModule _enoughProductsModuleMainCharacterCached;
        public InteractorEnoughProductsModule enoughProductsModuleMainCharacter
        {
            get
            {
                if (_enoughProductsModuleMainCharacterCached == null) _enoughProductsModuleMainCharacterCached = interactorCharactersCollection.mainCharacterView.GetModule<InteractorEnoughProductsModule>();
                return _enoughProductsModuleMainCharacterCached;
            }
        }
        
        private InteractorCharacterProductsCarrier _carrierMainCharacterCached;
        public InteractorCharacterProductsCarrier carrierMainCharacter
        {
            get
            {
                if (_carrierMainCharacterCached == null) _carrierMainCharacterCached = interactorCharactersCollection.mainCharacterView.GetComponentInChildren<InteractorCharacterProductsCarrier>();
                return _carrierMainCharacterCached;
            }
        }
        

        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        
        public int GetMainCharacterEnoughAmountFor(string productId)
        {
            int amount = enoughProductsModuleMainCharacter.GetEnoughAmountFor(productId);
            if (amount <= 0)
            {
                return amount;
            }
            amount -= carrierMainCharacter.Count(productId, false);
            if (amount < 0)
            {
                amount = 0;
            }
            
            return amount;
        }
    }
}
