using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using UnityEngine;

namespace GameCore.GameScene.Helper.Modules
{
    public class HelperSpeedModule : InteractorCharacterModule
    {
        [SerializeField] private float _speed;
        public float speed => _speed * _speedMultiplier;
        
        [SerializeField] private float _maxSpeed;
        public float maxSpeed => _maxSpeed;

        private float _speedMultiplier = 1;
        
        private HelperView _viewCached;
        public HelperView view => _viewCached ??= GetComponentInParent<HelperView>();
        
        public override void Construct()
        {
            view.taskModule.aiPath.maxSpeed = _speed;
        }

        public void SetSpeedMultiplier(float speedMultiplier)
        {
            _speedMultiplier = speedMultiplier;
            view.taskModule.aiPath.maxSpeed = speed;
        }
    }
}