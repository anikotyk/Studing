using GameCore.Common.Installers.SceneContext;
using GameCore.Common.Misc;
using GameCore.GameScene_Island.Saves;
using GameCore.GameScene.Helper;
using GameBasicsCore.Tools.Extensions;

namespace GameCore.GameScene_Island.Installers.SceneContext
{
    public class GameSceneIslandInstaller : GameSceneInstaller
    {
        public override string currSceneName => CommStr.GameScene_Island;
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.BindDefault<ShipStagesSaveData>();
            Container.BindDefault<TakenStartWoodsIslandSaveData>();
            Container.BindDefault<HelperController>();
        }
    }
}