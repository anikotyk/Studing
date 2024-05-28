using GameCore.Common.LevelItems.Animals;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Modules
{
    public class AnimalSpeedModule : InjCoreMonoBehaviour
    {
        [SerializeField] private float _speed;
        public float speed
        {
            get
            {
                if (_currentSpeed < 0) _currentSpeed = _speed;
                return _currentSpeed;
            }
        }

        [SerializeField] private float _maxSpeed;
        public float maxSpeed => _maxSpeed;

        private float _currentSpeed = -1;
        
        private AnimalView _viewCached;
        public AnimalView view => _viewCached ??= GetComponentInParent<AnimalView>(true);
        
        public override void Construct()
        {
            view.taskModule.aiPath.maxSpeed = _speed;
        }

        public void SetSpeed(float speed)
        {
            _currentSpeed = speed;
            view.taskModule.aiPath.maxSpeed = speed;
        }
        
        public void ResetSpeed()
        {
            _currentSpeed = -1;
            view.taskModule.aiPath.maxSpeed = _speed;
        }
    }
}