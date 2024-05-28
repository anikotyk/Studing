using DG.Tweening;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Character.Modules;
using GameCore.GameScene.LevelItems.Managers;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.Controllers.SceneContext
{
    public class GameStartController : ControllerInternal
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        [Inject, UsedImplicitly] public SpawnWoodInSeaManager spawnWoodInSeaManager { get; }
        
        private MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        
        private TheSaveProperty<int> _activeBuyObjectIndexSaveProperty;
        private TheSaveProperty<bool> _watchedCutsceneSaveProperty;
        
        public override void Construct()
        {
            base.Construct();
            
            _watchedCutsceneSaveProperty = new(CommStr.WatchedCutsceneArrive_Raft);
            _activeBuyObjectIndexSaveProperty = new(CommStr.ActiveBuyObjectIndex_Raft);
            
            initializeInOrderController.Add(Initialize, 1500);
        }

        private void Initialize()
        {
            ValidateSwim();
            ValidateWoodSpawn();
            ValidateCharacterPosition();
        }

        private void ValidateSwim()
        {
            if (_activeBuyObjectIndexSaveProperty.value <= 0)
            {
                mainCharacterView.GetModule<SwimModule>().SetSwim();
            }
        }

        private void ValidateWoodSpawn()
        {
            if (_activeBuyObjectIndexSaveProperty.value > 0)
            {
                spawnWoodInSeaManager.StartSpawn();
            }
            else if (_watchedCutsceneSaveProperty.value)
            {
                spawnWoodInSeaManager.ActivateTutorialWoods();
            }

            spawnWoodInSeaManager.startWood.onUsedAllWoods.Once(() =>
            {
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    mainCharacterView.GetModule<SwimModule>().SetNotSwim();
                },false).SetLink(spawnWoodInSeaManager.gameObject);
            });
        }

        private void ValidateCharacterPosition()
        {
            if (_activeBuyObjectIndexSaveProperty.value > 0 && _activeBuyObjectIndexSaveProperty.value <= 34)
            {
                mainCharacterView.transform.position = Vector3.zero;
            }

            if (!_watchedCutsceneSaveProperty.value)
            {
                mainCharacterView.transform.position = Vector3.zero + Vector3.back * 1.5f;
            }
        }
    }
}