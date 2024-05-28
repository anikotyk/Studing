using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Models;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Controllers
{
    public class UpgradesCheatsController :  ControllerInternal
    {
        [InjectOptional, UsedImplicitly] public ICheatPanelGUI cheatPanelGUI { get; }
        [Inject, UsedImplicitly] public UpgradesController upgradesController { get; }

        public override void Construct()
        {
            if (cheatPanelGUI != null)
            {
                cheatPanelGUI.onDraw.On(OnDraw).Priority(100);
            }
        }

        private void OnDraw(float width, float height)
        {
            GUILayout.BeginVertical("Box", GUILayout.Width(width));
            GUILayout.Label("Character upgrades");
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Full upgrade"))
            {
                var configs = GameplaySettings.def.upgradesGroup;
                foreach (var config in configs)
                {
                    UpgradePropertyModel model = upgradesController.GetModel(config);
                    while (!model.isMaxLevel)
                    {
                        model.TryUpgrade();
                    }
                }
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