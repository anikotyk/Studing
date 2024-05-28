using System;
using GameCore.GameScene.Audio;
using GameCore.ShipScene.Battle.Utilities;
using GameCore.ShipScene.Battle.Waves;
using GameCore.ShipScene.Weapons;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Cannons
{
    public class Cannon : BattleStartEndListener
    {
        [SerializeField] private int _capacity;
        [SerializeField] private Weapon _weapon;
    
        [Inject, UsedImplicitly] public PopSoundManager popSoundManager { get; }
        
        private int _currentChangeCount = 0;


        public int capacity => _capacity;
        public int currentChargeCount
        {
            get => _currentChangeCount;
            private set
            {
                int cachedChargeCount = _currentChangeCount;
                _currentChangeCount = Mathf.Clamp(value, 0, _capacity);
                if(_currentChangeCount != cachedChargeCount)
                    changeCountChanged.Dispatch(_currentChangeCount);
            }
        }

        public int freeSpace => _capacity - currentChargeCount;
        public bool isFull => _capacity <= currentChargeCount;

        public TheSignal addedCharge { get; } = new();
        public TheSignal<int> changeCountChanged { get; } = new();

        private void OnEnable()
        {
            _weapon.shot.On(RemoveCharge);
        }

        private void OnDisable()
        {
            _weapon.shot.Off(RemoveCharge);
        }
        
        public void AddChange()
        {
            if(isFull)
                return;
            popSoundManager.PlaySound();
            currentChargeCount++;
            addedCharge.Dispatch();
        }

        public void RemoveCharge()
        {
            currentChargeCount--;
            if(currentChargeCount == 0)
                DisableShooting();
        }

        protected override void OnBattleStarted(Wave wave)
        {
            if (currentChargeCount > 0)
                EnableShooting();
            else
                DisableShooting();
        }

        protected override void OnBattleEnded(Wave wave)
        {
            DisableShooting();
        }

        private void EnableShooting()
        {
            _weapon.collider.enabled = true;
        }

        private void DisableShooting()
        {
            _weapon.collider.enabled = false;
        }
    }
}