using GameCore.GameScene_Island.LevelItems.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.InteractItems
{
    public class ElevatorInteractItem : InteractItem
    {
        public override int priority { get; } = 4;
        
        private Elevator _elevatorCached;
        public Elevator elevator
        {
            get
            {
                if (_elevatorCached == null) _elevatorCached = GetComponent<Elevator>();
                return _elevatorCached;
            }
        }

        private bool _canInteract;

        public override void OnEnter(InteractorCharacterModel interactorModel)
        {
            base.OnEnter(interactorModel);
            _canInteract = true;
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (interactorModel is MainCharacterModel && _canInteract)
            {
                elevator.OnCharacterInteract(interactorModel);
                _canInteract = false;
            }
        }
        
        public override void OnExit(InteractorCharacterModel interactorModel)
        {
            if (interactorModel is MainCharacterModel)
            {
                elevator.OnCharacterStopInteract();
                _canInteract = true;
            }
        }
    }
}