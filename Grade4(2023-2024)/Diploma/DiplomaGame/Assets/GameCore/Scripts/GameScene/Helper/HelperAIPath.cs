using GameBasicsSignals;
using Pathfinding;
using UnityEngine;

namespace GameCore.GameScene.Helper
{
    public class HelperAIPath : AIPath
    {
        public bool isMoving { get; private set; } = true;
        public readonly TheSignal<Vector3> onNewDestination = new();
        public readonly TheSignal onReachedDestination = new();

        private Vector3 _targetDestination;
        private Transform _targetPoint;
        private bool _isReachedDispatched;

        private Vector3 _posLast;
        private float _magnitudeSamePlace = 0.02f;
        private float _timerSamePlace ;
        private float _timeSamePlaceDispatchReached = 2f;

        public bool isReachedDestination = false;

        public void SetDestination(Vector3 targetDestination)
        {
            if (destination == targetDestination)
            {
                onReachedDestination.Dispatch();
                return;
            }

            isReachedDestination = false;

            isMoving = true;
            _isReachedDispatched = false;
            destination =_targetDestination = targetDestination;
            onNewDestination.Dispatch(targetDestination);
        }

        protected override void Update()
        {
            base.Update();

            if (!_isReachedDispatched || _targetPoint)
            {
                if (_targetPoint != null)
                {
                    destination = _targetDestination = _targetPoint.position;
                }
                var dst = Vector3.Distance(transform.position, _targetDestination);
                if (dst <= endReachedDistance)
                {
                    _isReachedDispatched = true;
                    isReachedDestination = true;
                    onReachedDestination.Dispatch();
                }

                if (!_isReachedDispatched && !_targetPoint && (_posLast - transform.position).magnitude <= _magnitudeSamePlace)
                {
                    _timerSamePlace += Time.deltaTime;
                    if (_timerSamePlace >= _timeSamePlaceDispatchReached)
                    {
                        _isReachedDispatched = true;
                        isReachedDestination = true;
                        onReachedDestination.Dispatch();
                        _timerSamePlace = 0;
                    }
                }
                else
                {
                    _timerSamePlace = 0;
                }

                _posLast = transform.position;
            }
        }
        
        public void SetTarget(Transform targetPoint)
        {
            if (destination == targetPoint.position)
            {
                _isReachedDispatched = true;
                onReachedDestination.Dispatch();
                return;
            }

            isMoving = true;
            _isReachedDispatched = false;
            isReachedDestination = false;
            destination =_targetDestination = targetPoint.position;
            _targetPoint = targetPoint;
            onNewDestination.Dispatch(targetPoint.position);
        }

        public void RemoveTargetPoint()
        {
            _targetPoint = null;
        }

        public bool IsPathAvailable()
        {
            //TODO: 
            return true;
        }
    }
}