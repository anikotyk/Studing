using GameCore.Common.Controllers;
using GameCore.Common.LevelItems.Controllers;
using GameCore.Common.Saves;
using GameCore.GameScene.Controllers.SceneContext;
using GameCore.GameScene.Controllers.SceneContext.Products;
using GameCore.GameScene.Saves;
using GameCore.GameScene.States;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Installers.SceneContext;
using GameBasicsCore.Game.States;
using GameBasicsCore.Tools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext.CamAttention;
using GameBasicsSDK.Modules.IdleArcade.Controllers.VariousContext;
using Zenject;

namespace GameCore.Common.Installers.SceneContext
{
    public abstract class GameSceneInstaller : GameBasicsCoreSceneInstaller
    {
        [InjectOptional, UsedImplicitly] public ICheatPanelGUI cheatPanelGUI { get; }
        [Inject, UsedImplicitly] public GameSaveData gameSaveData { get; }
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            gameSaveData.value.lastPlayedScene = currSceneName;
            
            Container.BindInterfacesAndBaseTo<SceneStateHandler, GameSceneStateHandler>().AsSingle().NonLazy();
            Container.BindDefault<GameSceneStateMachine>();
            Container.BindDefault<ProductStoragesCollection>();
            
            Container.BindDefault<SellCollectController>();
            Container.BindDefault<ResourcesPopUpsController>();
            
            Container.BindDefault<PaidWoodSaveProperty>();
            Container.BindDefault<PaidProductsSaveData>();
            Container.BindDefault<PaidSCSaveProperty>();
            Container.BindDefault<UpgradesCheatsController>();
            Container.BindDefault<SavesCheatsController>();
            
            Container.BindDefault<TargetCameraOnObjectController>();
            Container.BindDefault<BuyObjectTaskController>();
            Container.BindDefault<TaskPanelController>();
            
            Container.BindDefault<ProductionController>();
            
            Container.BindDefault<GameSceneAdsController>();
            Container.BindDefault<CameraAttentionController>();
        }

        protected override void BindSeasonStepProgressDisplayBuilder()
        {
            
        }
    }
}