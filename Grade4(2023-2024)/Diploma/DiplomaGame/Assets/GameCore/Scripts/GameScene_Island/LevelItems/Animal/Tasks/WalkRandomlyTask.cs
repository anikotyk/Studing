using System.Collections;
using System.Linq;
using GameCore.Common.LevelItems.Animals;
using GameCore.GameScene.Helper.Tasks;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Tasks
{
    public class WalkRandomlyTask : HelperTask
    {
        protected override float maxTimeMoveToPoint => 40f;
        private AnimalView _view;
        private PointAnimalPath[] _pointsAssigned;
        private PointAnimalPath[] _pointsCached;
        public PointAnimalPath[] points
        {
            get
            {
                if (_pointsAssigned != null) return _pointsAssigned;
                if (_pointsCached == null) _pointsCached = GameObject.FindObjectsOfType<PointAnimalPath>(true).Where((item)=> !item.isPrivatePath).ToArray();
                return _pointsCached;
            }
        }

        private int _floor;

        public void Initialize(AnimalView view, int floor)
        {
            _view = view;
            _floor = floor;
        }
        
        public void Initialize(AnimalView view, PointAnimalPath[] animalPathPoints)
        {
            _view = view;
            _pointsAssigned = animalPathPoints;
        }

        
        protected override IEnumerator RunInternal()
        {
            while (isRunning)
            {
                var point = GetPointToWalk();
                if (point == null)
                {
                    _view.locomotionMovingModule.StopMovement();
                    float timer = 0;
                    while (isRunning && timer < 3f)
                    {
                        timer += Time.deltaTime;
                        yield return null;
                    } 
                    continue;
                }
                
                yield return null;
                Vector3 pos = point.transform.position;
                _view.taskModule.MoveTo(point.name, pos);
                yield return null;
                _view.locomotionMovingModule.StartMovement();
                float timer2 = 0f;
                while (!_view.taskModule.aiPath.reachedDestination && isRunning && timer2 < maxTimeMoveToPoint * 2f)
                {
                    yield return null;
                    timer2 += Time.deltaTime;
                }
                yield return null;
                _view.locomotionMovingModule.StopMovement();
                yield return new WaitForSeconds(0.5f);
            }
        }
        
        private PointAnimalPath GetPointToWalk()
        {
            if (points.FirstOrDefault(point => point.IsAvailable() && point.floor == _floor) == null) return null;
            int cnt = 0;
            while (cnt < 100)
            {
                cnt++;
                int index = Random.Range(0, points.Length);
                var item = points[index];
                if (item.IsAvailable() && item.floor == _floor) return item;
            }
            
            return null;
        }
    }
}