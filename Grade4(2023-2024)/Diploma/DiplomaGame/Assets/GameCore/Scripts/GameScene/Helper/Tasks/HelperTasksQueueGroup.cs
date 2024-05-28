using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace GameCore.GameScene.Helper.Tasks
{
    public class HelperTasksQueueGroup
    {
        private Queue<HelperTask> _tasksQueue = new Queue<HelperTask>();
        private MonoBehaviour _coroutinesMonoBehaviour;
        private HelperTask _currentTask;
        private HelperTask _defaultTask;
        private bool _isEnabled = true;

        public bool isRunningTask => _currentTask != null && _currentTask.isRunning;

        private Coroutine _currentTaskCoroutine;

        public void Initialize(MonoBehaviour coroutinesMonoBehaviour, HelperTask defaultTask)
        {
            _coroutinesMonoBehaviour = coroutinesMonoBehaviour;
            _defaultTask = defaultTask;
        }
        
        public void RunTask(HelperTask task)
        {
            _tasksQueue.Enqueue(task);
            if (_tasksQueue.Count <= 1)
            {
                RunTaskInternal(task);
            }
        }
        
        public void RunTaskAndCancelOther(HelperTask task)
        {
            StopTask();
            _tasksQueue.Clear();
            RunTask(task);
        }

        private void RunTaskInternal(HelperTask task)
        {
            if(!_coroutinesMonoBehaviour.gameObject.activeInHierarchy) return;
            if (!_isEnabled) return;
            if (_currentTask == _defaultTask)
            {
                StopTask();
            }

            _currentTask = task;
            _currentTask.onTaskStop.Once(()=>
            {
                if (!_tasksQueue.IsEmpty())
                {
                    _tasksQueue.Dequeue();
                }
                RunNextTask();
            });
            _currentTaskCoroutine = _coroutinesMonoBehaviour.StartCoroutine(task.Run());
        }

        private void RunNextTask()
        {
            if (!_tasksQueue.IsEmpty())
            {
                HelperTask task = _tasksQueue.Peek();
                RunTaskInternal(task);
            }
            else
            {
                RunDefaultTask();
            }
        }

        public void RunDefaultTask()
        {
            if(!_isEnabled) return;
            if(_currentTask == _defaultTask) return;
            if(!_coroutinesMonoBehaviour.gameObject.activeInHierarchy) return;
            _currentTask = _defaultTask;
            _currentTaskCoroutine = _coroutinesMonoBehaviour.StartCoroutine(_defaultTask.Run());
        }

        public void StopTask()
        {
            if (!isRunningTask) return;

            if (_currentTaskCoroutine != null)
            {
                _coroutinesMonoBehaviour.StopCoroutine(_currentTaskCoroutine);
            }
                
            _currentTask.Stop();
        }
        
        public void DisableTaskGroup()
        {
            _isEnabled = false;
            StopTask();
        }
        
        public void EnableTaskGroup()
        { 
            _isEnabled = true;
            RunNextTask();
        }

        public void RestartTask()
        {
            if (isRunningTask)
            {
                _currentTask.Reset();
                _currentTaskCoroutine = _coroutinesMonoBehaviour.StartCoroutine(_currentTask.Run());
            }
            else
            {
                RunNextTask();
            }
        }
    }
}