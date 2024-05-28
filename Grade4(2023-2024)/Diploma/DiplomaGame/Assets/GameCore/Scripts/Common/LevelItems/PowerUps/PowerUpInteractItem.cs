using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.Common.LevelItems.PowerUps
{
    public class PowerUpInteractItem : InteractItem
    {
        private PowerUpContainer _powerUpContainerCached;
        public PowerUpContainer powerUpContainer
        {
            get
            {
                if (_powerUpContainerCached == null) _powerUpContainerCached = GetComponent<PowerUpContainer>();
                return _powerUpContainerCached;
            }
        }

        public override int priority => -10000;

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            
        }

        public override void OnEnter(InteractorCharacterModel interactorModel)
        {
            if (interactorModel is MainCharacterModel)
            {
                powerUpContainer.OnCharacterEntered();
            }
        }

        public override void OnExit(InteractorCharacterModel interactorModel)
        {
            if (interactorModel is MainCharacterModel)
            {
                powerUpContainer.OnCharacterExited();
            }
        }
    }
}
