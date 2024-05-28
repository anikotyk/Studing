using DG.Tweening;
using Dreamteck.Splines;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Products
{
    public class MeatAnimalSplinableItem : MeatAnimalItem
    {
        [Inject, UsedImplicitly] public StallObject stallObject { get; }
        
        [SerializeField] private SplineFollower _splineFollower;
        public SplineFollower splineFollower => _splineFollower;
        
        [SerializeField] private float _defaultSpeed;
        [SerializeField] private float _hitSpeed;
        [SerializeField] private AnimatorParameterApplier _animIdle;
        [SerializeField] private AnimatorParameterApplier _animNotIdle;

        private bool _isReadyToStartMove = true;
        public bool isReadyToStartMove => _isReadyToStartMove;
        
        private void Awake()
        {
            if (splineFollower != null)
            {
                splineFollower.follow = false;
            }
        }

        public override void Construct()
        {
            base.Construct();

            ResetAnimal();
        }

        public void OnReachedEnd()
        {
            if (splineFollower != null)
            {
                splineFollower.follow = false;
            }
           
            _isReadyToStartMove = true;
        }

        public void SetSpline(SplineComputer splineComputer)
        {
            if (splineFollower != null)
            {
                splineFollower.spline = splineComputer;
            }
        }
        
        protected override void OnActivate()
        {
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
            if (splineFollower != null)
            {
                splineFollower.follow = true;
                splineFollower.followSpeed = _defaultSpeed;
            }
            _isReadyToStartMove = false;
        }

        public void InHitZone()
        {
            if (splineFollower != null)
            {
                splineFollower.followSpeed = _hitSpeed;
            }
            
            if (_hitSpeed <= 0f && _animIdle!=null)
            {
                _animIdle.Apply();
            }
        }

        public void NotInHitZone()
        {
            if (splineFollower != null)
            {
                splineFollower.followSpeed = _defaultSpeed;
            }
            if (_hitSpeed <= 0f && _animNotIdle!=null)
            {
                _animNotIdle.Apply();
            }
        }
        
        protected override void OnDie()
        {
            if (splineFollower != null)
            {
                splineFollower.follow = false;
            }
        }

        protected override void ResetAnimal()
        {
            base.ResetAnimal();
            
            if (splineFollower != null)
            {
                splineFollower.Restart();
                splineFollower.follow = false;
            }
           
            transform.localScale = Vector3.zero;
            
            if (_animNotIdle!=null)
            {
                _animNotIdle.Apply();
            }
            
            _isReadyToStartMove = true;
        }

        public bool CanStartMove()
        {
            return isReadyToStartMove && stallObject.gameObject.activeInHierarchy;
        }
    }
}