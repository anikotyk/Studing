using System;
using GameCore.GameScene.LevelItems.InteractItems;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Factories;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Battle
{
    public class BattleButtonUpgradeWindowHider : InjCoreMonoBehaviour
    {
        [SerializeField] private UpgradesPlatformInteractItem _interactItem;
        [SerializeField] private BattleButton _battleButton;
        
        private void OnEnable()
        {
            _interactItem.showed.On(OnUpgradeWindowShowed);
            _interactItem.hided.On(OnUpgradeWindowHided);
        }

        private void OnUpgradeWindowShowed()
        {
            _battleButton.Hide();
        }

        private void OnUpgradeWindowHided()
        {
            _battleButton.Show();
        }
    }
}