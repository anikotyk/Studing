using GameCore.Common.Misc;
using GameCore.ShipScene.Cutscenes;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Managers;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Cheats
{
    public class SceneSkipCheat : InjCoreMonoBehaviour
    {
        [InjectOptional, UsedImplicitly] public ICheatPanelGUI cheatPanelGUI { get; }
        [Inject, UsedImplicitly] public ShipCompletedCutscene shipCompletedCutscene { get; }

        public override void Construct()
        {
            base.Construct();

            if (cheatPanelGUI != null)
            {
                cheatPanelGUI.onDraw.On(OnDraw).Priority(100);
            }
        }

        private void OnDraw(float width, float height)
        {
            GUILayout.BeginVertical("Box", GUILayout.Width(width));
            GUILayout.Label("BuyCheats");
            Cheats();
            GUILayout.EndVertical();
        }

        protected virtual void Cheats()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Complete this scene"))
            {
                shipCompletedCutscene.StartCutscene();
                //sceneLoader.Load(CommStr.GameScene_Iceland);
            }

            GUILayout.EndHorizontal();
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (cheatPanelGUI != null)
            {
                cheatPanelGUI.onDraw.Off(OnDraw);
            }
        }
    }
}