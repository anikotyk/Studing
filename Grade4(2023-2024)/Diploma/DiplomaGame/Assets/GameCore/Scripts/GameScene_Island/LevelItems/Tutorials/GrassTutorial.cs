using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.LevelItems.Tutorials;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Tutorials
{
    public class GrassTutorial : RaftTutorial
    {
        [SerializeField] private CuttableZone _grass;
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        private MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        
        public override Vector3 targetPos => _grass.transform.position + Vector3.up * 0.5f;

        private TheSaveProperty<bool> _isGrassTutorialPassed;
        
        public override void Construct()
        {
            base.Construct();
            
            _isGrassTutorialPassed = new(CommStr.GrassTutorialPassed);
           
            if (!_isGrassTutorialPassed.value)
            {
                buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.On(TryStartTutorial);
                mainCharacterView.GetModule<CuttingModule>().onCutting.Once(CompleteTutorial);
                initializeInOrderController.Add(Initialize, 1000);
            }
            
        }

        private void Initialize()
        {
            TryStartTutorial(0);
        }

        private void TryStartTutorial(int index)
        {
            if (_grass.gameObject.activeInHierarchy && !_isGrassTutorialPassed.value)
            {
                StartTutorial();
                buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.Off(TryStartTutorial);
            }
        }

        public override void CompleteTutorial()
        {
            base.CompleteTutorial();

            _isGrassTutorialPassed.value = true;
        }
    }
}