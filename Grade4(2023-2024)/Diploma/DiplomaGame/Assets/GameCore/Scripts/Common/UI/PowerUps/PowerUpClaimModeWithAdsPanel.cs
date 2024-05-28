using GameCore.Common.Misc;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Presenters;
using GameBasicsCore.Game.Views.UI.Controls;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.UI.PowerUps
{
    public class PowerUpClaimModeWithAdsPanel : InjCoreMonoBehaviour
    {
        [SerializeField] private RvTktButtonsPanel _rvTktButtonsPanel;
        [Inject, UsedImplicitly] public RvTktUIControlsPresenter rvTktUIControlsPresenter { get; }

        public TheSignal onRvTktSuccess => _rvTktButtonsPanel.onSuccess;
        public TheSignal onRvTktClick => _rvTktButtonsPanel.onClick;
        
        public override void Construct()
        {
            _rvTktButtonsPanel.Init(rvTktUIControlsPresenter);
           
            _rvTktButtonsPanel.onSuccess.On(OnSuccess);
        }

        public void SetPlacementRvName(string rvName)
        {
            _rvTktButtonsPanel.SetPlacementName(rvName);
        }

        private void OnSuccess()
        {
            _rvTktButtonsPanel.SetInteractable(false);
        }
    }
}
