using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Character.Modules;
using GameCore.GameScene.LevelItems.Managers;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Tutorials
{
    public class StallTutorial : RaftTutorial
    {
        [SerializeField] private SharkTutorial _sharkTutorial;
        [Inject, UsedImplicitly] public StallObject stallObject { get; }
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        private MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;

        private ProductsCarrier carrier => mainCharacterView.GetModule<StackModule>().carrier;

        public override Vector3 targetPos => stallObject.acceptPoint.position + Vector3.up * 0.5f;

        private TheSaveProperty<bool> _isStallTutorialPassed;
        private TheSaveProperty<bool> _isSharkTutorialPassed;
        
        public override void Construct()
        {
            base.Construct();
            
            _isStallTutorialPassed = new(CommStr.StallTutorialPassed);
            _isSharkTutorialPassed = new(CommStr.SharkTutorialPassed);
           
            if (!_isStallTutorialPassed.value)
            {
                initializeInOrderController.Add(Initialize, 1000);
            }
        }

        private void Initialize()
        {
            if (!stallObject.gameObject.activeInHierarchy)
            {
                if (_isSharkTutorialPassed.value)
                {
                    stallObject.buyObject.onBuy.Once(TryStartTutorial);
                }
                else
                {
                    _sharkTutorial.onComplete.Once(TryStartTutorial);
                }
            }
            else
            {
                if (_isSharkTutorialPassed.value)
                {
                    TryStartTutorial();
                }
                else
                {
                    _sharkTutorial.onComplete.Once(TryStartTutorial);
                }
            }
        }

        private void TryStartTutorial()
        {
            DOVirtual.DelayedCall(0.05f, () =>
            {
                if (stallObject.gameObject.activeInHierarchy && !_isStallTutorialPassed.value)
                {
                    stallObject.carrier.onAddComplete.Once((_, _) =>
                    {
                        CompleteTutorial();
                    });
                    
                    carrier.onChange.On(OnChangedCharacterCarrier);
                    
                    if (carrier.count > 0)
                    {
                        StartTutorial();
                    }
                }
            },false).SetLink(gameObject);
        }

        private void OnChangedCharacterCarrier()
        {
            if (carrier.count <= 0)
            {
                if (isStarted)
                {
                    StopTutorial();
                }
            }
            else
            {
                if (!isStarted)
                {
                    StartTutorial();
                }
            }
        }

        public override void CompleteTutorial()
        {
            base.CompleteTutorial();

            _isStallTutorialPassed.value = true;
            
            carrier.onChange.Off(OnChangedCharacterCarrier);
        }
    }
}