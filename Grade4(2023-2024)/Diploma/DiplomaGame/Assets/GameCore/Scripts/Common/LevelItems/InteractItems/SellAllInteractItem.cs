using GameCore.Common.LevelItems.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.Common.LevelItems.InteractItems
{
    public class SellAllInteractItem : InteractItem
    {
        public override int priority { get; } = 2;

        private SellAllItem _sellAllItemCached;

        public SellAllItem sellAllItem
        {
            get
            {
                if (_sellAllItemCached == null) _sellAllItemCached = GetComponent<SellAllItem>();
                return _sellAllItemCached;
            }
        }
        
        public override void OnEnter(InteractorCharacterModel interactorModel)
        {
            base.OnEnter(interactorModel);
            if (interactorModel is MainCharacterModel)
            {
                sellAllItem.OnCharacterEnter();
            }
        }

        public override void OnExit(InteractorCharacterModel interactorModel)
        {
            base.OnExit(interactorModel);
            if (interactorModel is MainCharacterModel)
            {
                sellAllItem.OnCharacterExit();
            }
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            
        }
    }
}