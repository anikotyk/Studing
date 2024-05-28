using System;
using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene
{
    public class KillZone : MonoBehaviour
    {
        [SerializeField] private Transform _respawnPoint;
        [SerializeField] private Transform _player;
        [SerializeField] private Health _playerHealth;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Health health))
            {
                if (health == _playerHealth)
                {
                    _player.transform.position = _respawnPoint.position;
                    return;
                }
                health.Kill();
            }
        }
    }
}