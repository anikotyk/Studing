using System;

namespace GameCore.GameScene.Helper.Modules
{
    public class TaskProxy
    {
        public readonly string id;

        /// <summary>
        ///  When onDone or onInterrupt fire
        /// </summary>
        public bool complete;
        
        /// <summary>
        /// When internal stuff for task is done
        /// </summary>
        public bool finished { get; private set; }
        
        public Action onDone;
        public Action onInterrupt;
        public Action onFinish;

        public void Finished()
        {
            if (finished) return;
            finished = true;
            onFinish?.Invoke();
        }

        public TaskProxy(string id)
        {
            this.id = id;
        }
    }
}