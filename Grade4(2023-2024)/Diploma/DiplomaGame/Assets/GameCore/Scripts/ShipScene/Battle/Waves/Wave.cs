using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.ShipScene.Common;
using JetBrains.Annotations;
using GameBasicsCore.Game.API.Analytics;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Battle.Waves
{
    public class Wave : InjCoreMonoBehaviour
    {
        [SerializeField] private int _index;
        [SerializeField] private float _duration;
        [SerializeField] private float _timerStartDelay;
        [SerializeField] private WaveWinCondition _waveWinCondition;
        [Inject, UsedImplicitly] public BattleController battleController { get; }
        [Inject, UsedImplicitly] public LevelEventsManager levelEventsManager { get; }

        private TheSaveProperty<bool> _isFinishedProperty;

        private TheSaveProperty<bool> isFinishedProperty
            => _isFinishedProperty ??= new TheSaveProperty<bool>($"Wave_{_index}", false, ShipSceneConstants.saveFile);

        private bool _isTimerEnded = false;
        private WaveStatus _waveStatus = WaveStatus.None;

        public float timerStartDelay => _timerStartDelay;
        public bool isFinished => isFinishedProperty.value;
        public int index => _index;
        public bool isPlaying => _waveStatus == WaveStatus.Playing;
        public WaveStatus waveStatus => _waveStatus;
        public TheSignal<float> onProgressChanged { get; } = new();
        public TheSignal started { get; } = new();
        public TheSignal timerStarted { get; } = new();
        public TheSignal timerEnded { get; } = new();

        public TheSignal finished { get; } = new();
        public TheSignal failed { get; } = new();
        public TheSignal ended { get; } = new();
        
        public void StartWave()
        {
            if(isPlaying)
                return;
            
            levelEventsManager.OnLevelStepState(LevelEventsManager.EventState.Start, "Wave"+index);
            _isTimerEnded = false;
            _waveStatus = WaveStatus.Playing;
            started.Dispatch();
            _waveWinCondition.failed.Once(Fail);
            _waveWinCondition.finished.Once(Finish);
            DOVirtual.DelayedCall(_timerStartDelay, () =>
            {
                timerStarted.Dispatch();
                DOVirtual.Float(0, 1, _duration, onProgressChanged.Dispatch)
                    .OnComplete(() =>
                    {
                        _isTimerEnded = true;
                        OnTimerEnded();
                    }).SetId(this);
            }).SetId(this);
            
            OnStartWave();
        }
        protected virtual void OnStartWave(){}

        private void OnTimerEnded()
        {
            timerEnded.Dispatch();
            if(_waveStatus is WaveStatus.Victory or WaveStatus.Failure)
                EndWave();
        }
        
        private void Fail()
        {
            isFinishedProperty.value = false;
            _waveStatus = WaveStatus.Failure;
            levelEventsManager.OnLevelStepState(LevelEventsManager.EventState.Fail, "Wave"+index);
            failed.Dispatch();
            EndWave();
        }

        private void Finish()
        {
            isFinishedProperty.value = true;
            _waveStatus = WaveStatus.Victory;
            levelEventsManager.OnLevelStepState(LevelEventsManager.EventState.Complete, "Wave"+index);
            finished.Dispatch();
            EndWave();
        }
        

        private void EndWave()
        {
            DOTween.Kill(this);
            
            _waveWinCondition.failed.Off(Fail);
            _waveWinCondition.finished.Off(Finish);
            
            onProgressChanged.Dispatch(1);
            
            OnEndWave();
            
            battleController.EndBattle();
            ended.Dispatch();
        }
        protected virtual void OnEndWave(){}
        
        
    }
}