using GameBasicsCore.Game.Core;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Tutorials
{
    public abstract class RaftTutorial : InjCoreMonoBehaviour
    {
        public readonly TheSignal onStart = new();
        public readonly TheSignal onComplete = new();
        public readonly TheSignal onStop = new();

        private bool _isStarted;
        public bool isStarted => _isStarted;
        
        private bool _isCompleted;
        public bool isCompleted => _isCompleted;

        public virtual Vector3 targetPos => Vector3.zero;

        public virtual void StartTutorial()
        {
            if(_isStarted) return;
            onStart.Dispatch();
            _isStarted = true;
        }
        
        public virtual void CompleteTutorial()
        {
            _isCompleted = true;
            onComplete.Dispatch();
        }
        
        public virtual void StopTutorial()
        {
            if(!_isStarted) return;
            onStop.Dispatch();
            _isStarted = false;
        }
    }
}