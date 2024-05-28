using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Character.Modules;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Tutorials
{
    public class WaterFilterTutorial : RaftTutorial
    {
        [Inject, UsedImplicitly] public WaterFilterObject waterFilterObject { get; }
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        private MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        
        public override Vector3 targetPos => waterFilterObject.transform.position + Vector3.up * 0.5f;

        private TheSaveProperty<bool> _isWateringTutorialPassed;
        public bool isPassed => _isWateringTutorialPassed.value;
        
        public override void Construct()
        {
            base.Construct();
            
            _isWateringTutorialPassed = new(CommStr.WateringTutorialPassed);
           
            if (!_isWateringTutorialPassed.value)
            {
                buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.On(TryStartTutorial);
                mainCharacterView.GetModule<WateringModule>().onEndedWatering.Once(CompleteTutorial);
                initializeInOrderController.Add(Initialize, 1000);
            }
            
        }

        private void Initialize()
        {
            TryStartTutorial(0);
        }

        private void TryStartTutorial(int index)
        {
            if (GameObject.FindObjectOfType<WaterFilterObject>() && !_isWateringTutorialPassed.value)
            {
                StartTutorial();
                buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.Off(TryStartTutorial);
            }
        }

        public override void CompleteTutorial()
        {
            base.CompleteTutorial();

            _isWateringTutorialPassed.value = true;
        }
    }
}