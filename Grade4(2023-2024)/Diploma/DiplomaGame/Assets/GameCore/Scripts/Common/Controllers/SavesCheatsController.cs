using GameCore.Common.Saves;
using GameCore.ShipScene.Common;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Managers;
using GameBasicsCore.Game.Misc;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Controllers
{
    public class SavesCheatsController :  ControllerInternal
    {
        [InjectOptional, UsedImplicitly] public ICheatPanelGUI cheatPanelGUI { get; }
        [Inject, UsedImplicitly] public GameSaveData gameSaveData { get; }
        [Inject, UsedImplicitly] public SceneLoader sceneLoader { get; }
        

        public override void Construct()
        {
            if (cheatPanelGUI != null)
            {
                cheatPanelGUI.onDraw.On(OnDraw).Priority(0);
            }
        }

        private void OnDraw(float width, float height)
        {
            GUILayout.BeginVertical("Box", GUILayout.Width(width));
            GUILayout.Label("Saves");
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Delete all save files"))
            {
                //TODO:
                //ES3.DeleteFile(gameSaveData..path);
                ES3.DeleteFile(ShipSceneConstants.saveFilePath);
                sceneLoader.Restart();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (cheatPanelGUI != null)
            {
                cheatPanelGUI.onDraw.Off(OnDraw);
            }
        }
    }
}