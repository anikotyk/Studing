using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene_Island.LevelItems.Animal
{
    public class MeatAnimalInteractItem : InteractItem
    {
        public override int priority { get; } = 5;
        
        private MeatAnimalItem _meatAnimalItemCached;

        public MeatAnimalItem meatAnimalItem
        {
            get
            {
                if (_meatAnimalItemCached == null) _meatAnimalItemCached = GetComponent<MeatAnimalItem>();
                return _meatAnimalItemCached;
            }
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (!meatAnimalItem.isEnabled) return;
            if (interactorModel is not MainCharacterModel) return;
            
            meatAnimalItem.hub.Get<GCSgnl.SharkSignals.Interact>().Dispatch(interactorModel, meatAnimalItem);
        }
    }
}