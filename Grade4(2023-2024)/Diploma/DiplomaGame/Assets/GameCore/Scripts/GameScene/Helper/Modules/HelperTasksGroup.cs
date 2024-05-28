using GameCore.GameScene.Helper.Tasks;
using UnityEngine;

namespace GameCore.GameScene.Helper.Modules
{
    public class HelperTasksGroup
    {
        private MonoBehaviour _coroutinesMonoBehaviour;
        private HelperTask _currentTask;
        private bool _isEnabled = true;

        public bool isRunningTask => _currentTask != null && _currentTask.isRunning;

        private Coroutine _currentTaskCoroutine;

        public void Initialize(MonoBehaviour coroutinesMonoBehaviour)
        {
            _coroutinesMonoBehaviour = coroutinesMonoBehaviour;
        }
        
        public void RunTask(HelperTask task)
        {
            if (!_isEnabled) return;
            
            StopTask();
            
            _currentTask = task;
            _currentTaskCoroutine = _coroutinesMonoBehaviour.StartCoroutine(task.Run());
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
            RestartTask();
        }

        public void RestartTask()
        {
            //if (_currentTask == null) return;
            if (!isRunningTask) return;
            StopTask();
            RunTask(_currentTask);
        }
    }
}