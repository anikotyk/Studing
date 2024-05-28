using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene_Island.LevelItems.Animal;
using GameCore.GameScene_Island.LevelItems.Animal.Modules;
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
    public class InteractSheepTutorial : RaftTutorial
    {
        [SerializeField] private AnimalProductingView _sheep;
        public override Vector3 targetPos => _sheep.transform.position + Vector3.up * 0.5f;

        private TheSaveProperty<bool> _isSheepTutorialPassed;
        
        public override void Construct()
        {
            base.Construct();
            
            _isSheepTutorialPassed = new(CommStr.SheepTutorialPassed);
           
            if (!_isSheepTutorialPassed.value)
            {
                _sheep.productionModule.onBecomeAvailable.On(TryStartTutorial);
                _sheep.productionModule.onBecomeHungry.Once(CompleteTutorial);
            }
        }

        private void TryStartTutorial()
        {
            if (_sheep.gameObject.activeInHierarchy && _sheep.productionModule.isReadyToSpawn && !_isSheepTutorialPassed.value)
            {
                StartTutorial();
                _sheep.productionModule.onBecomeAvailable.Off(TryStartTutorial);
            }
        }

        public override void CompleteTutorial()
        {
            base.CompleteTutorial();

            _isSheepTutorialPassed.value = true;
        }
    }
}