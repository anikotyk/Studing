using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Battle.Waves
{
    public class WaveVisualization : InjCoreMonoBehaviour
    {
        [SerializeField] private Wave _wave;
        [SerializeField] private WaveView _waveView;

        [Inject, UsedImplicitly] public BattleController battleController { get; }
        
        private void OnEnable()
        {
            _waveView.Hide();
            _wave.started.On(_waveView.Show);
            _wave.timerEnded.On(_waveView.Hide);
        }

        public override void Construct()
        {
            battleController.restarted.On(_waveView.Hide);
        }
        
        private void OnDisable()
        {
            _wave.started.Off(_waveView.Show);
            _wave.timerEnded.Off(_waveView.Hide);
        }
    }
}