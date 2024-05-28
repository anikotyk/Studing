using GameCore.Common.Misc;
using GameCore.ShipScene.Cutscenes;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Managers;
using Zenject;

namespace GameCore.ShipScene.Common
{
    public class ShipSceneChanger : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public ShipCompletedCutscene shipCompletedCutscene { get; }
        [Inject, UsedImplicitly] public SceneLoader sceneLoader { get; }
        
        public override void Construct()
        {
            shipCompletedCutscene.onCutsceneCompleted.On(ChangeScene);
        }

        private void ChangeScene()
        {
            sceneLoader.Load(CommStr.GameScene_Iceland);
        }
    }
}