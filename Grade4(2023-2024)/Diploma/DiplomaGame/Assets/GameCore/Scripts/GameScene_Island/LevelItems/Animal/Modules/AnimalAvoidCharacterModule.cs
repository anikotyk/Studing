using System.Collections;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Modules
{
    public class AnimalAvoidCharacterModule : CoreMonoBehaviour
    {
        [SerializeField] private int _floor;
        private HogView _viewCached;
        public HogView view => _viewCached ??= GetComponentInParent<HogView>();
        
        private PointAnimalPath[] _pointsCached;
        public PointAnimalPath[] points
        {
            get
            {
                if (_pointsCached == null) _pointsCached = GameObject.FindObjectsOfType<PointAnimalPath>(true);
                return _pointsCached;
            }
        }

        public void OnEnterCharacter()
        {
            if (view.hogLogicModule.isTurnedOn && !view.hogLogicModule.isGoToTrap)
            {
                StartCoroutine(RunAwayCoroutine());
            }
        }

        private IEnumerator RunAwayCoroutine()
        {
            view.hogLogicModule.TurnOffHog(false);
            yield return null;
            view.speedModule.SetSpeed(view.speedModule.speed * 2.5f);
            var point = GetFarPoint();
            view.taskModule.MoveTo("RunAwayPoint", point.position);
            while (!view.taskModule.aiPath.reachedDestination) yield return null;
            yield return null;
            view.locomotionMovingModule.StopMovement();
            view.speedModule.ResetSpeed();
            view.hogLogicModule.TurnOnHog();
        }

        private Transform GetFarPoint()
        {
            Transform farPoint = points[0].transform;
            float maxDistance = 0;
            
            foreach (var point in points)
            {
                if (point.IsAvailable() && point.floor == _floor)
                {
                    float distance = Vector3.Distance(view.transform.position, point.transform.position);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        farPoint = point.transform;
                    }
                }
            }
            return farPoint;
        }
    }
}