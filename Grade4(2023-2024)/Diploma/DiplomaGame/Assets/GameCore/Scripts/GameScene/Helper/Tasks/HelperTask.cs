using System.Collections;
using GameBasicsSignals;
using UnityEngine;


namespace GameCore.GameScene.Helper.Tasks
{
    public abstract class HelperTask
    {
        protected virtual float maxTimeMoveToPoint => 20f;
        public bool isRunning { get; private set; }
        
        public readonly TheSignal onTaskStop = new();

        public virtual void Initialize()
        {
            
        }

        public IEnumerator Run()
        {
            if (isRunning)
            {
                Debug.LogWarning("Task already running!");
                yield break;
            }
            isRunning = true;
            
            yield return RunInternal();
            
            Stop();
        }

        protected abstract IEnumerator RunInternal();

        public void Stop()
        {
            if (!isRunning) return;
            isRunning = false;

            StopInternal();
            
            onTaskStop?.Dispatch();
        }
        
        public void Reset()
        {
            StopInternal();
        }

        protected virtual void StopInternal()
        {
            
        }
    }
}