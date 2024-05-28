using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Controllers.Ads.Single;
using GameBasicsCore.Game.Controllers.VCamControllers;
using GameBasicsCore.Game.Managers;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Windows.Menus;
using UnityEngine;
using Zenject;

#pragma warning disable 0649
namespace GameCore.GameScene.Controllers.SceneContext
{
    public class GameSceneAdsController : InSceneController
    {
        [InjectOptional, UsedImplicitly] public InterstitialAdUnitController interstitialAdUnitController { get; }
        [Inject, UsedImplicitly] public VirtualCameraController virtualCameraController { get; }
        [InjectOptional, UsedImplicitly] public ICheatPanelGUI cheatPanelGUI { get; }
        [Inject, UsedImplicitly] public SceneLoader sceneLoader { get; }
        
        private bool _isInterstitialsPaused = false;
        
        public override void Construct()
        {
            base.Construct();

            if (interstitialAdUnitController == null) return;
            
            hub.Get<NCSgnl.IUIWindowSignals.ShowStart>().On(window =>
            {
                if(!(window is GameplayUIMenuWindow)) interstitialAdUnitController.Pause(this);
            });
            hub.Get<NCSgnl.IUIWindowSignals.HideComplete>().On(window =>
            {
                if(!(window is GameplayUIMenuWindow)) interstitialAdUnitController.Unpause(this);
            });
            virtualCameraController.onCameraSwitchStart.On((_, _) => interstitialAdUnitController.Pause(this));
            virtualCameraController.onCameraSwitchComplete.On((_, _) => interstitialAdUnitController.Unpause(this));

            sceneLoader.projectHub.Get<NCSgnlPrj.BaseSignals.SceneLoad>().On(() =>
            {
                interstitialAdUnitController.UnpauseCompletely();
            });
            
            cheatPanelGUI.onDraw.On(DrawCheats);
        }

        public override void Dispose()
        {
            base.Dispose();
            cheatPanelGUI.onDraw.Off(DrawCheats);
            if (interstitialAdUnitController != null) interstitialAdUnitController.Unpause(this);
        }
        
        private void DrawCheats(float width, float height)
        {
            GUILayout.BeginVertical("Box", GUILayout.Width(width));

            GUILayout.BeginHorizontal();

            if (_isInterstitialsPaused)
            {
                if (GUILayout.Button("Unpause interstitials"))
                {
                    interstitialAdUnitController.Unpause(this);
                    _isInterstitialsPaused = false;
                }
            }
            else
            {
                if (GUILayout.Button("Pause interstitials"))
                {
                    interstitialAdUnitController.Pause(this);
                    _isInterstitialsPaused = true;
                }
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}