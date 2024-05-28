using System.Collections;
using GameCore.GameScene.Settings;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCore.GameScene.LevelItems.Managers
{
    public class JumpFishesManager : InjCoreMonoBehaviour
    {
        [SerializeField] private GameObject _fish;
        [SerializeField] private BoxCollider _spawnArea;
        
        public float intervalMin => GameplaySettings.def.jumpFishData.intervalMin;
        public float intervalMax => GameplaySettings.def.jumpFishData.intervalMax;

        private void Start()
        {
            StartCoroutine(JumpFishes());
        }

        private IEnumerator JumpFishes()
        {
            while (true)
            {
                float interval = Random.Range(intervalMin, intervalMax);
                yield return new WaitForSeconds(interval);
                
                _fish.transform.position = GetRandomPosition();
                _fish.gameObject.SetActive(true);
                
                yield return new WaitForSeconds(2f);
                _fish.gameObject.SetActive(false);
            }
        }

        private Vector3 GetRandomPosition()
        {
            return RandomPointInBounds(_spawnArea.bounds);
        }
        
        private static Vector3 RandomPointInBounds(Bounds bounds) {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }
    }
}