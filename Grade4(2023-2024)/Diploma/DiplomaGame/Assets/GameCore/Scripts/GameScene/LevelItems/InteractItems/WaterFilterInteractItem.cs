using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene.LevelItems.InteractItems
{
    public class WaterFilterInteractItem : InteractItem
    {
        private WaterFilterObject _waterFilterCached;
        
        public override int priority { get; } = 3;

        public WaterFilterObject waterFilter
        {
            get
            {
                if (_waterFilterCached == null) _waterFilterCached = GetComponent<WaterFilterObject>();
                return _waterFilterCached;
            }
        }

        public override bool CanInteract(InteractorCharacterModel interactorModel)
        {
            return base.CanInteract(interactorModel) && waterFilter.isEnabled && waterFilter.CanInteract(interactorModel.view); 
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (!waterFilter.isEnabled) return;
            waterFilter.OnInteracted(interactorModel.view);
        }
    }
}