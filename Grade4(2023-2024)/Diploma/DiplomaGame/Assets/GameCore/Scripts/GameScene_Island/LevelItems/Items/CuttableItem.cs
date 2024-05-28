using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EPOOutline;
using GameCore.Common.Controllers;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene_Island.Audio;
using GameCore.GameScene_Island.LevelItems.InteractItems;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using NaughtyAttributes;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Models;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class CuttableItem : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public UpgradesController upgradesController { get; }
        [Inject, UsedImplicitly] public CutSoundManager cutSoundManager { get; }
        [Inject, UsedImplicitly] public ProductionController productionController { get; }
        
        [SerializeField] private ProductDataConfig _spawnProductConfig;
        public ProductDataConfig spawnProductConfig => _spawnProductConfig;
        [SerializeField] private GameObject _visibleObject;
        [SerializeField] private GameObject _visibleObjectAfterCut;
        [SerializeField] private ParticleSystem _vfx;
        [SerializeField] private Transform _productsContainer;
        [SerializeField] private float _jumpRange = 0.75f;
        
        [SerializeField] private UpgradePropertyDataConfig _respawnTimeConfig;
        [SerializeField] private UpgradePropertyDataConfig _spawnCountConfig;
        [SerializeField] private bool _isToUseOutline;
        public bool UseOutline() => _isToUseOutline;
        [SerializeField, ShowIf("UseOutline")] private Outlinable _outlinable;
        [SerializeField] private int _cntStagesRespawn = 1;
        [SerializeField] private bool _isToStartRespawnOnBuy;

        private List<SellProduct> _productsAvailableForPickUp = new List<SellProduct>();
        public List<SellProduct> productsAvailableForPickUp => _productsAvailableForPickUp;
        
        private UpgradePropertyModel _modelRespawn;
        private UpgradePropertyModel _modelSpawnCount;
        
        public readonly TheSignal onSpawnedProducts = new();
        public readonly TheSignal onReadyToRespawn = new();

        private float _respawnTime;
        private int _spawnCount;

        private CuttableInteractItem _interactItemCached;
        
        public TheSignal onNotEnabled { get; } = new();
        public TheSignal onEnabled { get; } = new();

        public CuttableInteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<CuttableInteractItem>();
                return _interactItemCached;
            }
        }
        
        private Coroutine _respawnCoroutine;
        
        public override void Construct()
        {
            ValidateUPDCValues();

            if (_isToStartRespawnOnBuy)
            {
                GetComponentInParent<BuyObject>().onBuy.Once(() =>
                {
                    StartRespawn(1);
                });
            }
        }

        private void ValidateUPDCValues()
        {
            _modelRespawn = upgradesController.GetModel(_respawnTimeConfig);
            _respawnTime = (float) _modelRespawn.value;
            _modelRespawn.onChange.On((_) =>
            {
                _respawnTime = (float) _modelRespawn.value;
            });
            
            _modelSpawnCount = upgradesController.GetModel(_spawnCountConfig);
            _spawnCount = (int) _modelSpawnCount.value;
            _modelSpawnCount.onChange.On((_) =>
            {
                _spawnCount = (int) _modelSpawnCount.value;
            });
        }
        
        public void Cut(bool isToPlaySound = false, bool isToSpawnProduct = true)
        {
            OnNotReadyToCut();
            if (_visibleObjectAfterCut != null)
            {
                _visibleObjectAfterCut.gameObject.SetActive(true);
            }
           
            _visibleObject.transform.DOScale(Vector3.one * 0.01f, 0.35f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _visibleObject.SetActive(false);
            }).SetLink(gameObject);
            if (_vfx != null)
            {
                _vfx.Play();
            }

            if (isToPlaySound)
            {
                cutSoundManager.PlaySound();
            }

            if (isToSpawnProduct)
            {
                SpawnProducts();
            }
           

            onNotEnabled.Dispatch();
            OnCutted();
        }

        private void SpawnProducts()
        {
            int spawnCnt = (int)(_spawnCount * productionController.productionMultiplier);
            for (int i = 0; i < spawnCnt; i++)
            {
                var prod = spawnProductsManager.Spawn(_spawnProductConfig);
                prod.transform.SetParent(_productsContainer);
                Vector3 pos = _visibleObject.transform.position;
                pos.y = _productsContainer.position.y;
                prod.transform.position = pos;
                SellProduct sellProd = (prod as SellProduct);
                sellProd.TurnOffInteractItem();

                pos.x += Random.Range(-_jumpRange, _jumpRange);
                pos.z += Random.Range(-_jumpRange, _jumpRange);
                prod.transform.DORotate(Vector3.up * Random.Range(0, 360), 0.5f).SetLink(gameObject);
                prod.transform.DOJump(pos, 1.5f, 2, 0.5f).OnComplete(() =>
                {
                    sellProd.TurnOnInteractItem();
                    _productsAvailableForPickUp.Add(sellProd);
                    sellProd.onAddedToCarrier.Once(() =>
                    {
                        _productsAvailableForPickUp.Remove(sellProd);
                    });
                }).SetLink(gameObject);
            }
            
            DOVirtual.DelayedCall(0.5f, ()=>
            {
                onSpawnedProducts.Dispatch();
            }, false).SetLink(gameObject);
        }

        protected virtual void OnCutted()
        {
            onReadyToRespawn.Dispatch();
            StartRespawn();
        }

        public void StartRespawn(int respawnStageAtStart = 0)
        {
            _respawnCoroutine = StartCoroutine(RespawnCoroutine(respawnStageAtStart));
        }

        private IEnumerator RespawnCoroutine(int respawnStageAtStart = 0)
        {
            OnNotReadyToCut();
            int cntGrowthStages = _cntStagesRespawn;
            float timeGrowthStage = _respawnTime / cntGrowthStages;
            if (respawnStageAtStart > 0)
            {
                _visibleObject.transform.localScale = Vector3.one * (1f / cntGrowthStages) * respawnStageAtStart;
            }
            else
            {
                _visibleObject.transform.localScale = Vector3.one * 0.01f;
                _visibleObject.gameObject.SetActive(false);
            }
            
            for (int i = respawnStageAtStart; i < cntGrowthStages - 1; i++)
            {
                yield return new WaitForSeconds(timeGrowthStage);
                
                if (_visibleObjectAfterCut != null)
                {
                    _visibleObjectAfterCut.gameObject.SetActive(false);
                }
                
                _visibleObject.SetActive(true);
                
                _visibleObject.transform.DOScale(Vector3.one * (1f / cntGrowthStages) * (i + 1), 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
            }
            yield return new WaitForSeconds(timeGrowthStage + 0.5f);
            
            if (_visibleObjectAfterCut != null)
            {
                _visibleObjectAfterCut.gameObject.SetActive(false);
            }
            _visibleObject.SetActive(true);
            
            _visibleObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                interactItem.enabled = true;
                OnReadyToCut();
                onEnabled.Dispatch();
            }).SetLink(gameObject);
        }

        public void RespawnInternal()
        {
            if (_respawnCoroutine != null)
            {
                StopCoroutine(_respawnCoroutine);
            }
            
            _visibleObject.transform.localScale = Vector3.one;
            _visibleObject.SetActive(true);
            if (_visibleObjectAfterCut != null)
            {
                _visibleObjectAfterCut.gameObject.SetActive(false);
            }

            interactItem.enabled = true;
            OnReadyToCut();
            onEnabled.Dispatch();
        }

        private void OnReadyToCut()
        {
            if (UseOutline())
            {
                _outlinable.enabled = true;
            }
        }
        
        private void OnNotReadyToCut()
        {
            interactItem.enabled = false;
            if (UseOutline())
            {
                _outlinable.enabled = false;
            }
        }
    }
}