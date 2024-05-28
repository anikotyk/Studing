using System;
using System.Threading.Tasks;
using GameBasicsCore.Game.Core;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene.Helper.Modules
{
    public class HelperTaskModule : InjCoreMonoBehaviour
    {
        private HelperView _viewCached;
        public HelperView view => _viewCached ??= GetComponentInParent<HelperView>();

        private HelperAIPath _aiPathCached;
        public HelperAIPath aiPath => _aiPathCached ??= view.GetComponent<HelperAIPath>();

        public TaskProxy currTask { get; private set; }

        public TheSignal<TaskProxy> onNewTask { get; } = new();

        public override void Construct()
        {
            aiPath.onNewDestination.On(OnNewDestination);
            aiPath.onReachedDestination.On(OnReachedDestination);
        }

        private void OnNewDestination(Vector3 destination)
        {
            
        }

        private void OnReachedDestination()
        {
            if (currTask != null) OnTaskDone(currTask);
        }

        private TaskProxy NewTask(string id)
        {
            if (currTask != null) OnTaskInterrupt(currTask);
            currTask = new(id);
            onNewTask.Dispatch(currTask);
            return currTask;
        }

        private TaskProxy NewTask(string label, Action run)
        {
            NewTask(label);

            run.Invoke();

            return currTask;
        }

        public TaskProxy MoveTo(string id, Vector3 spotPosition)
        {
            async void Move()
            {
                // Need to wait one frame before SetDestination
                // because could be situation when character stands already in this position
                // and onReachedDestination will be called
                // and we get done task.
                await Task.Yield();
                
                aiPath.SetDestination(spotPosition);
                if (!aiPath.reachedDestination)
                {
                    view.locomotionMovingModule.StartMovement();
                }
            }

            return NewTask(id, Move);
        }
        
        public TaskProxy Follow(string id, Transform spot)
        {
            async void Move()
            {
                // Need to wait one frame before SetDestination
                // because could be situation when character stands already in this position
                // and onReachedDestination will be called
                // and we get done task.
                await Task.Yield();
                
                aiPath.SetTarget(spot);
                if (!aiPath.reachedDestination)
                {
                    view.locomotionMovingModule.StartMovement();
                }
            }

            return NewTask(id, Move);
        }
        
        public void StopFollow()
        {
            aiPath.RemoveTargetPoint();
        }

        private void OnTaskDone(TaskProxy task)
        {
            task.onDone?.Invoke();
            task.complete = true;
            if (currTask == task) currTask = null;
        }

        private void OnTaskInterrupt(TaskProxy task)
        {
            task.onInterrupt?.Invoke();
            task.complete = true;
            if (currTask == task) currTask = null;
        }
    }
}