using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameCore.GameScene.LevelItems.Managers
{
    public class SpawnWoodInSeaManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [InjectOptional, UsedImplicitly] public Raft raft { get; }
        
        [SerializeField] private ProductsGroupDataConfig _woodGroupDataConfig;
        [SerializeField] private Transform _container;
        [SerializeField] private StartWood _startWood;
        [SerializeField] private BoxCollider _frontSpawnArea;
        public StartWood startWood => _startWood;
        public List<WoodProduct> _woodProducts => GetComponentsInChildren<WoodProduct>().ToList();
        public GameplaySettings.SpawnWoodData spawnData => GameplaySettings.def.spawnWoodData;

        public void ActivateTutorialWoods()
        {
            _startWood.gameObject.SetActive(true);
            _startWood.onUsedAllWoods.Once(StartSpawn);
        }

        public void StartSpawn()
        {
            DOVirtual.DelayedCall(0.1f, () =>
            {
                SpawnAtStart();
            }, false).SetLink(gameObject);
            StartCoroutine(SpawnCoroutine());
        }

        private void SpawnAtStart()
        {
            for (int i = 0; i < spawnData.spawnAtStartCount; i++)
            {
                SpawnInSea(false);
            }
        }

        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnData.spawnWoodInterval);
                if (_woodProducts.Count < spawnData.maxSpawnedWoodCount)
                {
                    SpawnInSea();
                }
            }
        }

        private void SpawnInSea(bool isInFrontCollider = true)
        {
            Vector3 pos;
            
            if (isInFrontCollider)
            {
                pos = GetPositionForSpawnAtFront();
            }
            else
            {
                pos = GetRandomPositionForSpawn();
            }
            
            WoodProduct wood = Spawn(pos);
            wood.TurnOffInteractItem();
            wood.transform.localScale = Vector3.one * 0.01f;
            wood.transform.DOScale(Vector3.one*1.2f, 0.5f).SetEase(Ease.OutBack).OnComplete(() => 
            {
                wood.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InOutBack).OnComplete(() => 
                {
                    wood.StartSeaIdleAnim();
                    wood.TurnOnInteractItem();
                }).SetLink(gameObject);
            }).SetLink(gameObject);
        }
        
        private WoodProduct Spawn(Vector3 position, ProductDataConfig productConfig = null)
        {
            if (productConfig == null)
            {
                int index = Random.Range(0, _woodGroupDataConfig.Count);
                productConfig = _woodGroupDataConfig[index];
            }

            ProductView wood = spawnProductsManager.Spawn(productConfig, position);
            wood.transform.SetParent(_container);
            
            return wood as WoodProduct;
        }
        
        private Vector3 GetPositionForSpawnAtFront()
        {
            return RandomPointInBounds(_frontSpawnArea.bounds);
        }
        
        private Vector3 GetRandomPositionForSpawn()
        {
            Vector3 pos = Vector3.zero;
            bool isInsideTheRaft = false;
            int cnt = 0;
            
            do
            {
                float x = Random.Range(-spawnData.spawnPosRange, spawnData.spawnPosRange);
                float z = Random.Range(-spawnData.spawnPosRange, spawnData.spawnPosRange);
                pos = new Vector3(x, _container.position.y, z);
                if (raft)
                {
                    isInsideTheRaft = raft.IsInsideTheRaft(pos);
                }
                cnt++;
            } while (isInsideTheRaft && cnt < 100);
            
            return pos;
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