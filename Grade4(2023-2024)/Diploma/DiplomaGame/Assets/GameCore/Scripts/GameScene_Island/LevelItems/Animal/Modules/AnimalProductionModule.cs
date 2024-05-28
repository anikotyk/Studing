using DG.Tweening;
using GameCore.Common.Controllers;
using GameCore.Common.LevelItems.Animals;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.Common.Misc;
using GameCore.GameScene_Island.UI;
using GameCore.GameScene.Audio;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using GameBasicsSignals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameCore.GameScene_Island.LevelItems.Animal.Modules
{
    public class AnimalProductionModule : CoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public ContainerAnimalProducts containerAnimalProducts { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public StallSoundManager stallSoundManager { get; }
        [Inject, UsedImplicitly] public ProductionController productionController { get; }

        [SerializeField] private ProductDataConfig _productDataConfig;
        [SerializeField] private int _minCntToSpawn = 1;
        [SerializeField] private int _maxCntToSpawn = 3;
        [SerializeField] private float _timeBeforeReadyToSpawn = 1;
        [SerializeField] private InteractItem _interactItem;
        public InteractItem interactItem => _interactItem;
        [SerializeField] private Transform _popUpPoint;
        
        [SerializeField] private Transform _interactPoint;
        public Transform interactPoint => _interactPoint;
        [SerializeField] private bool _isSoundEnabled = true;
        [SerializeField] private AudioSource _soundReadyToSpawnProducts;

        private ImagePopUpView _animalReadyPopUp;
        private ProgressPopUp _animalWaitingReadyPopUp;
        private Tween _popUpTweenAnimalReady;
        private Tween _popUpTweenAnimalWaitingReady;
        
        private AnimalView _viewCached;
        public AnimalView view => _viewCached ??= GetComponentInParent<AnimalView>();
        
        private bool _isReadyToSpawn = false;
        private bool _isHungry = true;
        public bool isHungry => _isHungry;
        public bool isReadyToSpawn => _isReadyToSpawn;
        
        public TheSignal onBecomeHungry { get; } = new();
        public TheSignal onBecomeAvailable { get; } = new();

        protected virtual void Awake()
        {
            BecomeUnavailableSpawn();
        }

        public void Eat()
        {
            _isHungry = false;
            WaitReadyToSpawn();
        }

        protected virtual void BecomeAvailableSpawn()
        {
            if (_soundReadyToSpawnProducts != null && _isSoundEnabled)
            {
                _soundReadyToSpawnProducts.Play();
            }

            _interactItem.enabled = true;
            HidePopUpAnimalWaitingReady();
            ShowPopUpAnimalReady();
            OnCharacterClose();
            onBecomeAvailable.Dispatch();
        }
        
        private void BecomeUnavailableSpawn()
        {
            _interactItem.enabled = false;
            HidePopUpAnimalReady();
        }

        public void GetProducts(InteractorCharacterModel interactorModel)
        {
            BecomeUnavailableSpawn();
            
            Vector3 dir = (interactorModel.view.transform.position - transform.position).normalized;
            dir.y = 0;
            Vector3 movePos = transform.position + dir * 0.55f;
            Vector3 rot = Quaternion.LookRotation(-1 * dir, Vector3.up).eulerAngles;
            
            interactorModel.view.GetModule<AnimalInteractModule>().OnStartInteract();
            
            interactorModel.view.transform.DORotate(rot, 0.35f).SetLink(gameObject);
            interactorModel.view.transform.DOMove(movePos, 0.35f).OnComplete(() =>
            {
                InteractorOnGetProducts(interactorModel);
                EffectOnStartGetProducts();
                
                DOVirtual.DelayedCall(0.75f, () =>
                {
                    view.animationsModule.PlayGetProduct();
                    EffectOnGetProduction();
                    DOVirtual.DelayedCall(0.25f, () =>
                    {
                        stallSoundManager.PlaySound();
                        SpawnProducts();
                        view.transform.DOPunchScale(Vector3.one * 0.1f, 0.25f).SetLink(view.gameObject);
                        DOVirtual.DelayedCall(0.25f, () =>
                        {
                            interactorModel.view.GetModule<AnimalInteractModule>().OnEndInteract();
                            OnCharacterFar();
                            AfterGetProducts();
                        }, false).SetLink(gameObject);
                    }, false).SetLink(gameObject);
                }, false).SetLink(gameObject);
            }).SetLink(gameObject);
        }

        protected virtual void EffectOnGetProduction()
        {
            
        }
        
        protected virtual void EffectOnStartGetProducts()
        {
            
        }

        protected virtual void AfterGetProducts()
        {
            _isHungry = true;
            onBecomeHungry.Dispatch();
        }

        protected virtual void InteractorOnGetProducts(InteractorCharacterModel interactorModel)
        {
            
        }

        private void SpawnProducts()
        {
            int cnt = Random.Range(_minCntToSpawn, _maxCntToSpawn + 1);
            cnt = (int)(cnt * productionController.productionMultiplier);
            for (int i = 0; i < cnt; i++)
            {
                var prod = spawnProductsManager.Spawn(_productDataConfig);
                prod.transform.SetParent(containerAnimalProducts.transform);
                Vector3 pos = transform.position;
                //pos.y = containerAnimalProducts.transform.position.y;
                prod.transform.position = pos + Vector3.up * 0.05f;
                
                SellProduct sellProd = (prod as SellProduct);
                sellProd.TurnOffInteractItem();
                
                float range = 1f;
                pos.x += Random.Range(-range, range);
                pos.z += Random.Range(-range, range);
                
                prod.transform.DORotate(Vector3.up * Random.Range(0, 360), 0.5f).SetLink(gameObject);
                prod.transform.DOJump(pos, 1.5f, 2, 0.5f).OnComplete(() =>
                {
                    sellProd.TurnOnInteractItem();
                    containerAnimalProducts.AddProduct(sellProd);
                }).SetLink(gameObject);
            }
        }

        private void WaitReadyToSpawn()
        {
            BecomeUnavailableSpawn();
            _isReadyToSpawn = false;
            ShowPopUpAnimalWaitingReady();
            float timer = 0;
            DOVirtual.DelayedCall(_timeBeforeReadyToSpawn, () =>
            {
                _isReadyToSpawn = true;
                if (!_isHungry)
                {
                    BecomeAvailableSpawn();
                }
            }, false).OnUpdate(() =>
            {
                timer += Time.deltaTime;
                _animalWaitingReadyPopUp.SetProgress(timer/_timeBeforeReadyToSpawn);
            }).SetLink(gameObject);
        }
        
        private void ShowPopUpAnimalReady()
        {
            if (_animalReadyPopUp == null)
            {
                _animalReadyPopUp = popUpsController.SpawnUnderMenu<ImagePopUpView>("AnimalReadyPopUp");
                _animalReadyPopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
                _animalReadyPopUp.worldSpaceConverter.followWorldObject = _popUpPoint.transform;
                _animalReadyPopUp.SetSprite(_productDataConfig.icon);
            }
           
            _animalReadyPopUp.transform.localScale = Vector3.zero;
            _animalReadyPopUp.gameObject.SetActive(true);
            
            if (_popUpTweenAnimalReady != null)
            {
                _popUpTweenAnimalReady.Kill();
            }
            _popUpTweenAnimalReady = _animalReadyPopUp.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }
        
        private void ShowPopUpAnimalWaitingReady()
        {
            if (_animalWaitingReadyPopUp == null)
            {
                _animalWaitingReadyPopUp = popUpsController.SpawnUnderMenu<ProgressPopUp>("AnimalWaitingReadyPopUp");
                _animalWaitingReadyPopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
                _animalWaitingReadyPopUp.worldSpaceConverter.followWorldObject = _popUpPoint.transform;
                _animalWaitingReadyPopUp.SetSprite(_productDataConfig.icon);
            }
           
            _animalWaitingReadyPopUp.transform.localScale = Vector3.zero;
            _animalWaitingReadyPopUp.gameObject.SetActive(true);
            
            if (_popUpTweenAnimalWaitingReady != null)
            {
                _popUpTweenAnimalWaitingReady.Kill();
            }
            _popUpTweenAnimalWaitingReady = _animalWaitingReadyPopUp.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }

        
        private void HidePopUpAnimalReady()
        {
            if (_animalReadyPopUp == null) return;
            
            if (_popUpTweenAnimalReady != null)
            {
                _popUpTweenAnimalReady.Kill();
            }
            
            _popUpTweenAnimalReady = _animalReadyPopUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                _animalReadyPopUp.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }
        
        private void HidePopUpAnimalWaitingReady()
        {
            if (_animalWaitingReadyPopUp == null) return;
            
            if (_popUpTweenAnimalWaitingReady != null)
            {
                _popUpTweenAnimalWaitingReady.Kill();
            }
            
            _popUpTweenAnimalWaitingReady = _animalWaitingReadyPopUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                _animalWaitingReadyPopUp.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }

        public void OnCharacterClose()
        {
            view.taskModule.aiPath.canMove = false;
            view.locomotionMovingModule.StopMovement();
        }
        
        public void OnCharacterFar()
        {
            view.taskModule.aiPath.canMove = true;
            view.locomotionMovingModule.StartMovement();
        }
    }
}