using DG.Tweening;
using GameCore.Common.Misc;
using GameBasicsCore.Game.Core;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.Common.LevelItems.Items.HittableItems
{
    public class HittableItem : InjCoreMonoBehaviour, IHittable
    {
        [SerializeField] private BoxCollider _interactCollider;
        public BoxCollider colliderInteract => _interactCollider;
        protected virtual float enableDelay => 0.5f;
        public virtual float canHitAngle => 60f;
        
        private bool _isEnabled = true;
        public bool isEnabled => _isEnabled;

        protected Tween enableTween;

        public Transform view => transform;
        public virtual Vector3 helperPosition => view.position + Vector3.back*0.5f;
        public virtual CharacterTools.HittingToolType toolType => CharacterTools.HittingToolType.Axe;
        private readonly TheSignal _onTurnOff = new();
        public TheSignal onTurnOff => _onTurnOff;
       
        private readonly TheSignal _onTurnOn = new();
        public TheSignal onTurnOn => _onTurnOn;
        
        public virtual void OnHit(float multipler = 1)
        {
            SetDisabled();
            enableTween = DOVirtual.DelayedCall(enableDelay, () =>
            {
                SetEnabled();
            }).SetLink(gameObject);
            
            EffectOnHit();
        }

        protected virtual void EffectOnHit()
        {
            
        }

        protected void SetEnabled()
        {
            _isEnabled = true;
        }
        
        protected void SetDisabled()
        {
            _isEnabled = false;
        }
    }
}