using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.ShipScene.Battle.Waves;
using NaughtyAttributes;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsSignals;
using GameBasicsSignals.Api;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Battle
{
    public class BattleController : InjCoreMonoBehaviour
    {
        [SerializeField] private List<Wave> _waves;
        [SerializeField] private Health _playerHealth;

        private List<Wave> _wavesCached;        
        public List<Wave> waves => _wavesCached ??= _waves.OrderBy(x => x.index).Distinct().ToList();

        public Health playerHealth => _playerHealth;

        private Wave _currentWave;
        public Wave nextWave => waves.Find(x => x.isFinished == false);
        public bool isStarted { get; private set; } = false;

        public TheSignal<Wave> started { get; } = new();
        public TheSignal<Wave> ended { get; } = new();
        public TheSignal restarted { get; } = new();
        public TheSignal wavesEnded { get; } = new();

        [Button()]
        public void StartBattle()
        {
            if(isStarted)
                return;
            if (_waves.Count == 0)
            {
                Debug.LogError("No waves added");
                return;
            }

            if (nextWave == null)
            {
                wavesEnded.Dispatch();
                Debug.LogError("There are no unfinished waves");
                return;
            }
            
            AstarPath.active.Scan();
            
            _playerHealth.HealToMax();
            isStarted = true;
            _currentWave = nextWave;
            _currentWave.StartWave();
            started.Dispatch(_currentWave);
        }

        public void EndBattle()
        {
            if(isStarted == false)
                return;
            isStarted = false;
            ended.Dispatch(_currentWave);
            _currentWave = null;
            if(nextWave == null)
                wavesEnded.Dispatch();
        }

        
        public void Restart()
        {
            playerHealth.Revive();
            restarted.Dispatch();
        }

    }
}