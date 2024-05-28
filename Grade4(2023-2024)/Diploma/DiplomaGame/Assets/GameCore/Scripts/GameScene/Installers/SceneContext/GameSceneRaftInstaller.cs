using GameCore.Common.Installers.SceneContext;
using GameCore.Common.Misc;
using GameCore.GameScene.Controllers.SceneContext;
using GameCore.GameScene.Helper;
using GameCore.GameScene.Saves;
using GameBasicsCore.Tools.Extensions;

namespace GameCore.GameScene.Installers.SceneContext
{
    public class GameSceneRaftInstaller : GameSceneInstaller
    {
        public override string currSceneName => CommStr.GameScene_Raft;
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.BindDefault<GameStartController>();
            Container.BindDefault<TakenStartWoodsSaveData>();
            Container.BindDefault<HelperController>();
        }
    }
}