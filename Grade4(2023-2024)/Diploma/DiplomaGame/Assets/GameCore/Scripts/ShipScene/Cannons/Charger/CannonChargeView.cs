using System;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.GameBasicsCore.Plugins.Tools.GameBasicsTools.LittleHelpers;
using TMPro;
using UnityEngine;

namespace GameCore.ShipScene.Cannons
{
    public class CannonChargeView : InjCoreMonoBehaviour
    {
        [SerializeField] private Cannon _cannon;
        [SerializeField] private CooldownPulseAnimation _cooldownPulseAnimation;
        [SerializeField] private TMP_Text _text;

        private void OnEnable()
        {
            _cannon.changeCountChanged.On(OnChargeCountChanged);
            OnChargeCountChanged(_cannon.currentChargeCount);
        }

        private void OnDisable()
        {
            _cannon.changeCountChanged.Off(OnChargeCountChanged);
        }

        private void OnChargeCountChanged(int charges)
        {
            _text.text = $"{charges}/{_cannon.capacity}";
            _cooldownPulseAnimation.PlaySimple();
        }
    }
}