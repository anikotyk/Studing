using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.Common.LevelItems.Items;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene.Audio;
using GameCore.GameScene.Misc;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Tweens;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Products
{
    public class FruitTreeItem : InjCoreMonoBehaviour, IWatering
    {
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [SerializeField] private Transform _characterMovePoint;
        [SerializeField] private Transform _interactPoint;
        public Transform interactPoint => _interactPoint;
        [SerializeField] private Transform _treeVisual;
        [SerializeField] private Transform _camPoint;
        public Transform camPoint => _camPoint;
        [SerializeField] private Transform _spotVisual;
        [SerializeField] private Transform _containerFallFruits;
        [SerializeField] private Transform _containerCurrentFruits;
        [SerializeField] private SellProduct[] _fruits;
        [SerializeField] private Transform _unwateredPopUpPoint;
        [SerializeField] private WaterFilterObject _waterFilterObject;
        [Inject, UsedImplicitly] public SignalHub hub { get; }
        [Inject, UsedImplicitly] public StallSoundManager stallSoundManager { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        
        public Transform pointWatering => _treeVisual;

        private float _angleRotate = 15f;
        private int _fallFruitsParts = 6;
        public float respawnTime => GameplaySettings.def.fruitTreeData.respawnTime;

        private List<SellProduct> _currentFruits = new List<SellProduct>();
        private List<SellProduct> _fruitsAvailableForPickUp = new List<SellProduct>();
        public List<SellProduct> fruitsAvailableForPickUp => _fruitsAvailableForPickUp;
        
         
        private InteractItem _interactItemCached;
        public InteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<InteractItem>();
                return _interactItemCached;
            }
        }
        
        private bool _isEnabled = true;
        public bool isEnabled => _isEnabled;
        
        private Tween _popUpTween;

        private InteractorCharacterModel _interactorCharacter;

        private int _cntFruitsFall = 0;

        private ImagePopUpView _unwateredPopUp;
        private bool _isUnwatered;
        
        private Tween _treeScaleTween;
        
        public readonly TheSignal onEnabled = new();
        public readonly TheSignal onEndHitting = new();
        public readonly TheSignal onFruitsReadyToCollect = new();

        public override void Construct()
        {
            base.Construct();

            SpawnFruitsInternal(true);
        }

        public void OnWatered()
        {
            if (_isUnwatered)
            {
                HideUnwateredPopUp();
                StartRespawn();
            }
            _isUnwatered = false;
        }

        public void StartHitting(InteractorCharacterModel characterModel)
        {
            if(!characterModel.view.GetModule<FruitTreeHittingModule>().CanInteract()) return;
            
            TreeHittingNotAvailable();
            
            _cntFruitsFall = 0;
            _interactorCharacter = characterModel;
            
            var mover = characterModel.view.GetModule<MoveToStandOnPointCharacterModule>();
            mover.MoveAndLookInItsDirection(_characterMovePoint, 0.5f);
            
            characterModel.view.GetModule<FruitTreeHittingModule>().onMove.On((moveCoef)=>
            {
                MoveTreeAndCharacter(moveCoef, characterModel);
            });

            characterModel.view.GetModule<FruitTreeHittingModule>().PreStartHitting();

            DOVirtual.DelayedCall(0.5f, () =>
            {
                characterModel.view.GetModule<FruitTreeHittingModule>().StartHitting(this);
            }, false).SetLink(gameObject);
        }

        private void MoveTreeAndCharacter(float coef ,InteractorCharacterModel characterModel)
        {
            _treeVisual.rotation = Quaternion.Euler(new Vector3(coef * _angleRotate, 0, 0));
            Vector3 rot = _characterMovePoint.rotation.eulerAngles;
            rot.z = coef * _angleRotate;
            characterModel.view.transform.rotation = Quaternion.Euler(rot);

            if (coef >= 1 || coef <= -1)
            {
                FallFruits(coef, characterModel is MainCharacterModel);
            }
        }

        private void FallFruits(float dir, bool isToPlaySound = false)
        {
            if (_treeScaleTween != null)
            {
                _treeScaleTween.Kill();
                _treeVisual.localScale = Vector3.one;
            }
            _treeScaleTween = _treeVisual.DOPunchScale(Vector3.one*0.05f, 0.25f).SetLink(gameObject);
            
            float cnt = _fruits.Length / _fallFruitsParts;
            if (_currentFruits.Count > cnt && _currentFruits.Count < cnt * 2)
            {
                cnt = _currentFruits.Count;
            }
            cnt = Mathf.Min(_currentFruits.Count, cnt);
            
            for (int i = 0; i < cnt; i++)
            {
                FallFruit(_currentFruits[0], dir);
                _currentFruits.Remove(_currentFruits[0]);
            }

            if (isToPlaySound)
            {
                stallSoundManager.PlaySound();
            }

            _cntFruitsFall++;

            if (_cntFruitsFall >= _fallFruitsParts)
            {
                OnAllFruitsFall();
            }
        }

        private void FallFruit(SellProduct apple, float dir)
        {
            apple.transform.parent = _containerFallFruits;

            Vector3 jumpPos = apple.transform.position;
            jumpPos.y = _containerFallFruits.position.y;
            jumpPos.z += dir * Random.Range(-0.25f, 0.75f) * -1f;
            jumpPos.x += Random.Range(-0.5f, 0.5f);
            apple.transform.DOJump(jumpPos, 1, 2,0.5f).OnComplete(() =>
            {
                apple.TurnOnOutline();
            }).SetLink(gameObject);
        }

        private void OnAllFruitsFall()
        {
            onEndHitting.Dispatch();
            _treeVisual.DORotate(Vector3.zero, 0.25f);
            _interactorCharacter.view.transform.DORotate(new Vector3(0, 180, 0), 0.25f).SetLink(gameObject);
            _interactorCharacter.view.GetModule<FruitTreeHittingModule>().onMove.OffAll();

            DOVirtual.DelayedCall(0.5f, ()=>
            {
                foreach (var apple in _containerFallFruits.GetComponentsInChildren<SellProduct>())
                {
                    apple.TurnOnInteractItem();
                    _fruitsAvailableForPickUp.Add(apple);
                    apple.onAddedToCarrier.Once(() =>
                    {
                        _fruitsAvailableForPickUp.Remove(apple);
                    });
                }
                onFruitsReadyToCollect.Dispatch();
                SetUnwatered();
            }, false).SetLink(gameObject);
        }

        private void StartRespawn()
        {
            SpawnFruitsInternal();
            StartCoroutine(RespawnFruitsCoroutine(respawnTime));
        }

        private void SpawnFruitsInternal(bool isActive = false)
        {
            foreach (var fruit in _fruits)
            {
                var newFruit = spawnProductsManager.Spawn(fruit.dataConfig);
                newFruit.transform.SetParent(_containerCurrentFruits);
                newFruit.transform.localPosition = fruit.transform.localPosition;
                newFruit.transform.localRotation = fruit.transform.localRotation;
                newFruit.transform.localScale = fruit.transform.localScale;
                newFruit.gameObject.SetActive(isActive);
                SellProduct sellProduct = newFruit as SellProduct;
                sellProduct.TurnOffInteractItem();
                sellProduct.TurnOffOutline();
                _currentFruits.Add(sellProduct);
            }
        }

        private IEnumerator RespawnFruitsCoroutine(float time)
        {
            float interval = time / _fruits.Length;
            for (int i = 0; i < _fruits.Length; i++)
            {
                yield return new WaitForSeconds(interval);
                _currentFruits[i].gameObject.SetActive(true);
                _currentFruits[i].transform.localScale = Vector3.one * 0.01f;
                _currentFruits[i].transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.OutBack).SetLink(gameObject);
            }
            yield return new WaitForSeconds(0.5f);
            TreeHittingAvailable();
        }

        private void TreeHittingAvailable()
        {
            _isEnabled = true;
            interactItem.enabled = true;
            _spotVisual.gameObject.SetActive(true);
            _spotVisual.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
            onEnabled.Dispatch();
        }
        
        private void TreeHittingNotAvailable()
        {
            _isEnabled = false;
            interactItem.enabled = false;
            _spotVisual.transform.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _spotVisual.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }
        
        private void SetUnwatered()
        {
            if (_isUnwatered) return;
            _isUnwatered = true;
            
            hub.Get<GCSgnl.WateringSignals.NeedsWater>().Dispatch(_waterFilterObject);

            SetupUnwateredPopUp();
        }
        
        private void SetupUnwateredPopUp()
        {
            if (_unwateredPopUp == null)
            {
                _unwateredPopUp = popUpsController.SpawnUnderMenu<ImagePopUpView>("Unwatered");
                _unwateredPopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
                _unwateredPopUp.worldSpaceConverter.followWorldObject = _unwateredPopUpPoint.transform;
            }
           
            _unwateredPopUp.transform.localScale = Vector3.zero;
            _unwateredPopUp.gameObject.SetActive(true);
            
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            _popUpTween = _unwateredPopUp.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }

        private void HideUnwateredPopUp()
        {
            if (_unwateredPopUp == null) return;
            
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            
            var popUp = _unwateredPopUp;
            _popUpTween = popUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                popUp.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }
    }
}