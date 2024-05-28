using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Animals.FightAnimal;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.LevelItems.Tutorials;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Tutorials
{
    public class BearForCharacterFurTutorial : RaftTutorial
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        protected InteractorCharacterModel mainCharacterModel => interactorCharactersCollection.mainCharacterView.model;
        
        [SerializeField] private FightAnimalView _fightAnimalView;
        [SerializeField] private BuyObject _buyObjectComplete;
        [SerializeField] private ProductDataConfig _requiredProductDC;
        [SerializeField] private RaftTutorial _workbenchTutorial;
        public override Vector3 targetPos => _fightAnimalView.transform.position + Vector3.up * 1f;

        private Tween _checkTween;

        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 2000);
        }

        private void Initialize()
        {
            if (_buyObjectComplete.isInBuyMode)
            {
                StartTutorial();
            }
            else if (!_buyObjectComplete.isBought)
            {
                _buyObjectComplete.onSetInBuyMode.Once(StartTutorial);
            }

            _buyObjectComplete.onBuy.Once(() =>
            {
                if(_checkTween!=null) _checkTween.Kill();
                mainCharacterModel.productCarriersController.onChange.Off(CheckForBearRespawn);
                _fightAnimalView.onDeactivate.Off(OnFightAnimalDeactivate);
                
                _fightAnimalView.Deactivate();
                
                CompleteTutorial();
            });
        }

        public override void StartTutorial()
        {
            base.StartTutorial();
            
            if(_checkTween!=null) _checkTween.Kill();
            mainCharacterModel.productCarriersController.onChange.Off(CheckForBearRespawn);
            _fightAnimalView.Activate();
            _fightAnimalView.transform.localScale = Vector3.one * 0.01f;
            _fightAnimalView.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
            _fightAnimalView.onDeactivate.Once(OnFightAnimalDeactivate);
        }

        private void OnFightAnimalDeactivate()
        {
            StopTutorial();
            _workbenchTutorial.StartTutorial();
            mainCharacterModel.productCarriersController.onChange.On(CheckForBearRespawn);
        }

        private void CheckForBearRespawn()
        {
            if(_checkTween!=null) _checkTween.Kill();
            _checkTween = DOVirtual.DelayedCall(0.5f, () =>
            {
                if (!mainCharacterModel.productCarriersController.IsCarryingProduct(_requiredProductDC) && !_buyObjectComplete.isBought && !spawnProductsManager.IsContainsProduct(_requiredProductDC))
                {
                    _workbenchTutorial.StopTutorial();
                    StartTutorial();
                }else if (!_workbenchTutorial.isStarted)
                {
                    _workbenchTutorial.StartTutorial();
                }
            }, false).SetLink(gameObject);
        }
    }
}