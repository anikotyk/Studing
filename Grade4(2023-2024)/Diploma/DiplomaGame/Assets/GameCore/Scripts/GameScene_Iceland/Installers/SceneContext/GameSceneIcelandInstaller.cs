using GameCore.Common.Installers.SceneContext;
using GameCore.Common.Misc;
using GameCore.GameScene_Iceland.Saves;
using GameCore.GameScene.Helper;
using GameBasicsCore.Tools.Extensions;

namespace GameCore.GameScene_Iceland.Installers.SceneContext
{
    public class GameSceneIcelandInstaller : GameSceneInstaller
    {
        public override string currSceneName => CommStr.GameScene_Iceland;
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.BindDefault<HelperController>();
            
            Container.BindDefault<PaidWoodForBoatHuntingSaveProperty>();
            Container.BindDefault<PaidProductsForBoatHuntingSaveData>();
            Container.BindDefault<PaidSCForBoatHuntingSaveProperty>();
        }
    }
}
            