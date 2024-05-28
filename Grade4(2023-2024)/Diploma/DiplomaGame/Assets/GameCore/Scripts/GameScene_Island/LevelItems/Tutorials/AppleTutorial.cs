using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.LevelItems.Tutorials;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Tutorials
{
    public class AppleTutorial : RaftTutorial
    {
        [FormerlySerializedAs("_appleTreeItem")] [SerializeField] private FruitTreeItem fruitTreeItem;
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        private MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        
        public override Vector3 targetPos => fruitTreeItem.interactPoint.position + Vector3.up * 0.5f;

        private TheSaveProperty<bool> _isAppleTutorialPassed;
        
        public override void Construct()
        {
            base.Construct();
            
            _isAppleTutorialPassed = new(CommStr.AppleTutorialPassed);
           
            if (!_isAppleTutorialPassed.value)
            {
                buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.On(TryStartTutorial);
                fruitTreeItem.onEndHitting.Once(CompleteTutorial);
                initializeInOrderController.Add(Initialize, 1000);
            }
            
        }

        private void Initialize()
        {
            TryStartTutorial(0);
        }

        private void TryStartTutorial(int index)
        {
            if (fruitTreeItem.gameObject.activeInHierarchy && !_isAppleTutorialPassed.value)
            {
                StartTutorial();
                buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.Off(TryStartTutorial);
            }
        }

        public override void CompleteTutorial()
        {
            base.CompleteTutorial();

            _isAppleTutorialPassed.value = true;
        }
    }
}