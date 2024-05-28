using System.Collections;
using System.Collections.Generic;
using GameCore.Common.Settings;
using GameBasicsCore.Game.Controllers;
using UnityEngine;

namespace GameCore.Common.Controllers.PowerUps
{
    public class PowerUpsController : ControllerInternal
    {
        private Queue<PowerUpController> _queueSpawnPowerUps = new Queue<PowerUpController>();
        private List<PowerUpController> _initializedControllers = new List<PowerUpController>();
        private bool isEnabled = true;
        private Coroutine _spawnCoroutine = null;

        public void AddToSpawnQueue(PowerUpController controller)
        {
            if (!isEnabled) return;
            
            if (!_initializedControllers.Contains(controller))
            {
                InitializeController(controller);
            }
            _queueSpawnPowerUps.Enqueue(controller);
            
            if (_queueSpawnPowerUps.Count <= 1)
            {
                bool res = SpawnPowerUp(controller);
                if(!res) _queueSpawnPowerUps.Dequeue();
            }
        }
        
        private void SpawnNextPowerUp()
        {
            if (_spawnCoroutine != null)
            {
                level.StopCoroutine(_spawnCoroutine);
            }
            _spawnCoroutine = level.StartCoroutine(SpawnWithDelay());
        }

        private IEnumerator SpawnWithDelay()
        {
            yield return new WaitForSeconds(PowerUpsSettings.def.minDelayBetweenPowerUps);
            SpawnWithoutDelay();
        }

        private void SpawnWithoutDelay()
        {
            _queueSpawnPowerUps.Dequeue();
            
            if (_queueSpawnPowerUps.Count <= 0) return;
            
            bool res = SpawnPowerUp(_queueSpawnPowerUps.Peek());
            if(!res) SpawnWithoutDelay();
        }

        private bool SpawnPowerUp(PowerUpController controller)
        {
            return controller.TrySpawnPowerUp();
        }

        private void Deactivate()
        {
            _queueSpawnPowerUps.Clear();
            isEnabled = false;
            if (_spawnCoroutine != null)
            {
                level.StopCoroutine(_spawnCoroutine);
            }
        }

        private void InitializeController(PowerUpController controller)
        {
            _initializedControllers.Add(controller);
            controller.onPowerUpGet.On(()=>{SpawnNextPowerUp();});
            controller.onPowerUpDestroyed.On(()=>{SpawnNextPowerUp();});
        }
    }
}
