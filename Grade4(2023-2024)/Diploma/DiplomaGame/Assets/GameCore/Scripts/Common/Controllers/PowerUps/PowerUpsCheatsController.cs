using System.Collections.Generic;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Controllers;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Controllers.PowerUps
{
    public class PowerUpsCheatsController : ControllerInternal
    {
        [InjectOptional, UsedImplicitly] public ICheatPanelGUI cheatPanelGUI { get; }
        
        private List<PowerUpController> _powerUpControllers = new List<PowerUpController>();
        
        public override void Construct()
        {
            cheatPanelGUI.onDraw.On(DrawCheats).Priority(1);
        }

        public override void Dispose()
        {
            base.Dispose();
            cheatPanelGUI.onDraw.Off(DrawCheats);
        }

        public void AddController(PowerUpController controller)
        {
            _powerUpControllers.Add(controller);
        }

        private void DrawCheats(float width, float height)
        {
            if(_powerUpControllers.Count <= 0) return;
            
            GUILayout.BeginVertical("Box", GUILayout.Width(width));
            GUILayout.Label($"Power Ups");
            GUILayout.BeginHorizontal();

            int cnt = 0;
            foreach (var controller in _powerUpControllers)
            {
                if (GUILayout.Button(controller.cheatBtnName))
                {
                    controller.SpawnPowerUpCheat();
                }

                cnt++;
                if (cnt >= 3)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    cnt = 0;
                }
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}