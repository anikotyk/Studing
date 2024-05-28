using GameCore.GameScene_Island.LevelItems.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene_Island.LevelItems.InteractItems
{
    public class ShipPartInteractItem : InteractItem
    {
        public override int priority { get; } = 4;
        
        private ShipPart _shipPartCached;
        public ShipPart shipPart
        {
            get
            {
                if (_shipPartCached == null) _shipPartCached = GetComponent<ShipPart>();
                return _shipPartCached;
            }
        }
        
        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (interactorModel is MainCharacterModel)
            {
                shipPart.OnTaken();
            }
        }
    }
}