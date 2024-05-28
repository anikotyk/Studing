using GameCore.GameScene_Island.LevelItems.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene_Island.LevelItems.InteractItems
{
    public class OldFilterInteractItem : InteractItem
    {
        public override int priority { get; } = 4;
        
        private OldFilter _oldFilterCached;
        public OldFilter oldFilter
        {
            get
            {
                if (_oldFilterCached == null) _oldFilterCached = GetComponent<OldFilter>();
                return _oldFilterCached;
            }
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (interactorModel is MainCharacterModel)
            {
                oldFilter.OnInteracted();
            }
        }
    }
}