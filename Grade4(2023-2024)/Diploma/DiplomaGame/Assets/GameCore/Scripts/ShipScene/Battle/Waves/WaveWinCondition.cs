using GameBasicsSignals;
using UnityEngine;

namespace GameCore.ShipScene.Battle.Waves
{
    public abstract class WaveWinCondition : MonoBehaviour
    {
        [SerializeField] private Wave _wave;
        [SerializeField] private bool _failOnPlayerDeath;
        
        public Wave wave => _wave;
        public TheSignal finished { get; } = new();
        public TheSignal failed { get; } = new();

        public bool isEnded { get; private set; } = false;

        private void OnEnable()
        {
            _wave.started.On(OnStartListen);
            _wave.ended.On(OnEndListen);
        }

        private void OnStartListen()
        {
            if (_failOnPlayerDeath)
                wave.battleController.playerHealth.died.On(Fail);
            StartListen();
        }
        protected abstract void StartListen();

        private void OnEndListen()
        {
            if(isEnded)
                return;
            wave.battleController.playerHealth.died.Off(Fail);
            EndListen();
        }
        protected abstract void EndListen();

        public void Victory()
        {
            isEnded = true;
            EndListen();
            finished.Dispatch();
        }

        public void Fail()
        {
            isEnded = true;
            failed.Dispatch();
            EndListen();
        }
    }
}