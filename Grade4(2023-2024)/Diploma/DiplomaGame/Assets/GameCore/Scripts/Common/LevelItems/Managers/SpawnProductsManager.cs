using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Managers
{
    public class SpawnProductsManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SignalHub hub { get; }
        [SerializeField] private Transform _container;
        public Transform container => _container;

        private List<ProductView> _productsPool = new List<ProductView>();
        
        public ProductView Spawn(ProductDataConfig productConfig)
        {
            var prod = _productsPool.FirstOrDefault((item) => item.id == productConfig.id);
            _productsPool.Remove(prod);
            
            if (prod == null)
            {
                prod = Instantiate(productConfig.viewPrefab);
                SubscribeToRelease(prod);
                if (prod is SellProduct)
                {
                    (prod as SellProduct).Initialize(hub);
                }
            }
            
            prod.gameObject.SetActive(true);
            prod.transform.localScale = Vector3.one;
            
            if (prod is SellProduct)
            {
                (prod as SellProduct).TurnOnInteractItem();
                (prod as SellProduct).TurnOnOutline();
            }

            return prod;
        }

        public void SubscribeToRelease(ProductView prod)
        {
            prod.onRelease.On(() =>
            {
                ReleaseProduct(prod);
            });
        }
        
        public ProductView Spawn(ProductDataConfig productConfig, Vector3 pos)
        {
            var prod = Spawn(productConfig);
            prod.transform.SetParent(_container);
            prod.transform.position = pos;
            prod.transform.rotation = GetRandomRotation();

            return prod;
        }
        
        public List<ProductView> SpawnBunchAtPoint(ProductDataConfig productConfig, Vector3 pos, int cnt, float jumpRange)
        {
            List<ProductView> spawnedProducts = new List<ProductView>();
            for (int i = 0; i < cnt; i++)
            {
                var prod = Spawn(productConfig, pos);
                if (prod is SellProduct)
                {
                    (prod as SellProduct).TurnOffInteractItem();
                }
                
                Vector3 direction = Vector3.forward * Random.Range(-jumpRange, jumpRange) +
                                    Vector3.right * Random.Range(-jumpRange, jumpRange);
                JumpEffectProduct(prod, pos, direction);
                spawnedProducts.Add(prod);
            }
            
            return spawnedProducts;
        }
        
        public void SpawnBunchAtPoint(ProductDataConfig productConfig, Vector3 pos, int cnt, float xMinRange, float xMaxRange, float zMinRange, float zMaxRange)
        {
            for (int i = 0; i < cnt; i++)
            {
                var prod = Spawn(productConfig, pos);
                if (prod is SellProduct)
                {
                    (prod as SellProduct).TurnOffInteractItem();
                }
                
                Vector3 direction = Vector3.forward * Random.Range(zMinRange, zMaxRange) +
                                    Vector3.right * Random.Range(xMinRange, xMaxRange);
                JumpEffectProduct(prod, pos, direction);
            }
        }
        
        private void JumpEffectProduct(ProductView prod, Vector3 pos, Vector3 direction)
        {
            Vector3 jumpPos = pos + direction;
            prod.transform.DOJump(jumpPos, 1.5f, 1, 0.35f).SetEase(Ease.Linear).OnComplete(() =>
            {
                prod.transform.DOJump(jumpPos+direction.normalized * 0.2f, 0.35f, 1, 0.25f).OnComplete(() =>
                {
                    if (prod is SellProduct)
                    {
                        (prod as SellProduct).TurnOnInteractItem();
                    }
                }).SetLink(gameObject);
            }).SetLink(gameObject);
        }


        private void ReleaseProduct(ProductView prod)
        {
            if (!_productsPool.Contains(prod))
            {
                _productsPool.Add(prod);
            }
            prod.gameObject.SetActive(false);
            prod.transform.SetParent(_container);
            if (prod is SellProduct)
            {
                (prod as SellProduct).ResetView();
            }
        }
        
        private Quaternion GetRandomRotation()
        {
            int y = Random.Range(0, 360);
            Vector3 angle = new Vector3(0, y, 0);
            
            return Quaternion.Euler(angle);
        }

        public bool IsContainsProduct(ProductDataConfig productConfig)
        {
            return _container.GetComponentsInChildren<ProductView>().FirstOrDefault((item) => item.id == productConfig.id);
        }
    }
}