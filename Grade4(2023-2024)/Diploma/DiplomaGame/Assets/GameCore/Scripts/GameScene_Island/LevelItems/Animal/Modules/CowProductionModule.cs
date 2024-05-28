using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene_Island.LevelItems.Animal.Modules
{
    public class CowProductionModule : AnimalProductionModule
    {
        protected override void InteractorOnGetProducts(InteractorCharacterModel interactorModel)
        {
            interactorModel.view.GetModule<AnimalInteractModule>().OnCowInteract();
        }
        
        protected override void EffectOnStartGetProducts()
        {
            base.EffectOnStartGetProducts();
            view.animator.enabled = false;
        }

        protected override void AfterGetProducts()
        {
           base.AfterGetProducts();
           view.animator.enabled = true;
        }
    }
}