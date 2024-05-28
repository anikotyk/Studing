using GameCore.GameScene_Island.LevelItems.Animal;
using GameCore.GameScene.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene.LevelItems.InteractItems
{
    public class MeatAnimalSplinableInteractItem : MeatAnimalInteractItem
    {
        public override int priority { get; } = 5;

        public MeatAnimalSplinableItem meatAnimalSplinableItem => meatAnimalItem as MeatAnimalSplinableItem;
        
        public override void OnEnter(InteractorCharacterModel interactorModel)
        {
            if (interactorModel is not MainCharacterModel) return;
            meatAnimalSplinableItem.InHitZone();
        }
        
        public override void OnExit(InteractorCharacterModel interactorModel)
        {
            if (interactorModel is not MainCharacterModel) return;
            meatAnimalSplinableItem.NotInHitZone();
        }
    }
}