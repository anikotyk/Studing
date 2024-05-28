using GameCore.Common.Misc;
using GameCore.ShipScene.Currency;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Settings.GameCore.Modules.GameCurrency;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Cheats
{
    public class SoftCurrencyCheat : InjCoreMonoBehaviour
    {
        [InjectOptional, UsedImplicitly] public ICheatPanelGUI cheatPanelGUI { get; }
        [Inject] private ShipCoinsCollectModel _shipCoinsCollectModel;
        private GameCurrencyGCSModule settings => ShipSceneSettings.def.currencyModule;
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

            if (GUILayout.Button("add +10 coins"))
            {
                _shipCoinsCollectModel.Earn(10, settings.currencyName, settings.currencyName);
            }
            if (GUILayout.Button("add +1000 coins"))
            {
                _shipCoinsCollectModel.Earn(1000, settings.currencyName, settings.currencyName);
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