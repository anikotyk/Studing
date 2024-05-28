using GameBasicsCore.Game.Core;
using GameBasicsSignals;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Common.UI.PowerUps
{
    public class PowerUpClaimModePanel : InjCoreMonoBehaviour
    {
        [SerializeField] private Button _btnClaim;

        public readonly TheSignal onClaimClick = new();

        private void Start()
        {
            _btnClaim.onClick.AddListener(() =>
            {
                _btnClaim.interactable = false;
                onClaimClick.Dispatch();
            });
        }
    }
}
