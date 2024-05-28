using System;
using GameBasicsSDK.GameBasicsCore.Plugins.Tools.GameBasicsTools.LittleHelpers;
using UnityEngine;

namespace GameCore.ShipScene.Cannons
{
    public class CannonChargeFx : MonoBehaviour
    {
        [SerializeField] private Cannon _cannon;
        [SerializeField] private CooldownPulseAnimation _cooldownPulseAnimation;

        private void OnEnable()
        {
            _cannon.addedCharge.On(OnAddCharge);
        }

        private void OnDisable()
        {
            _cannon.addedCharge.Off(OnAddCharge);
        }

        private void OnAddCharge()
        {
            _cooldownPulseAnimation.PlaySimple();
        }
    }
}