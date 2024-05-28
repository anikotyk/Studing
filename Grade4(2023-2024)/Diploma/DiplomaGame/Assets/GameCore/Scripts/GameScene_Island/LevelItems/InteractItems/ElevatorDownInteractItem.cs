using GameCore.GameScene_Island.LevelItems.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.InteractItems
{
    public class ElevatorDownInteractItem : InteractItem
    {
        [SerializeField] private Elevator _elevator;
        public override int priority { get; } = 4;

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (interactorModel is MainCharacterModel)
            {
                _elevator.OnCharacterInteractDown(interactorModel);
            }
        }
    }
}