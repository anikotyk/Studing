using System;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene
{
    public class DeadCharacterMovementDisabler : InjCoreMonoBehaviour
    {
        [SerializeField] private Health _health;

        [Inject, UsedImplicitly] public MainCharacterSimpleWalkerController walker { get; }
        
        private void OnEnable()
        {
            _health.died.On(OnDied);
            _health.revived.On(OnRevived);
        }

        private void OnDied()
        {
            walker.TurnOffMovement();
        }

        private void OnRevived()
        {
            walker.TurnOnMovement();
        }
    }
}