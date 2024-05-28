using DG.Tweening;
using UnityEngine;

namespace GameCore.Common.LevelItems.Items.HittableItems
{
    public class SimpleProductingHittableItem : ProductingHittableItem
    {
        [SerializeField] private ParticleSystem _vfx;
        [SerializeField] private Transform _visibleObject;
        public Transform visibleObject => _visibleObject;

        protected override void EffectOnHit()
        {
            base.EffectOnHit();
            
            _vfx.Play();
            _visibleObject.DOPunchRotation(Vector3.right * 10f, 0.5f).SetLink(gameObject);
        }
    }
}