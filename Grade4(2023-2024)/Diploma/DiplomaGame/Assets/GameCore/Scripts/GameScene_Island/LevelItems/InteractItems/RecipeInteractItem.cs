using GameCore.GameScene_Island.LevelItems.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene_Island.LevelItems.InteractItems
{
    public class RecipeInteractItem : InteractItem
    {
        public override int priority { get; } = 4;
        
        private Recipe _recipeCached;
        public Recipe recipe
        {
            get
            {
                if (_recipeCached == null) _recipeCached = GetComponent<Recipe>();
                return _recipeCached;
            }
        }
        
        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (interactorModel is MainCharacterModel)
            {
                recipe.OnTaken();
            }
        }
    }
}