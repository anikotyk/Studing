using GameCore.GameScene_Island.LevelItems.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene_Island.LevelItems.InteractItems
{
    public class MillInteractItem : InteractItem
    {
        public override int priority { get; } = 3;
        
        private MillItem _millItemCached;
        public MillItem millItem
        {
            get
            {
                if (_millItemCached == null) _millItemCached = GetComponent<MillItem>();
                return _millItemCached;
            }
        }
        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (!millItem.isEnabled) return;
            millItem.OnInteracted(interactorModel);
        }
    }
}