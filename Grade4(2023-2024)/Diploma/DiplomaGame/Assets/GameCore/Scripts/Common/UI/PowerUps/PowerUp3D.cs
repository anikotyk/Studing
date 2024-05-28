using DG.Tweening;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Tools.Extensions;
using UnityEngine;

namespace GameCore.Common.UI.PowerUps
{
    public class PowerUp3D : InjCoreMonoBehaviour
    {
        private float _size;
        
        public override void Construct()
        {
            _size = transform.localScale.x;
            transform.localScale = Vector3.zero;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            DOTween.Kill(this);
        }

        public Sequence Appear(Sequence seq)
        {
            seq = DOTween.Sequence();
            seq.SetId(this);
            seq.Append(transform.DOScale(_size, 0.5f).SetId(this));
            seq.Join(transform.DOLocalRotate(Vector3.up * 660, 0.5f, RotateMode.FastBeyond360)
                .SetRelative()
                .SetEase(Ease.OutCirc)
                .SetId(this));
            seq.AppendInterval(0.05f);
            
            seq.AppendCallback(InfiniteRotate);

            return seq;
        }

        public void InfiniteRotate()
        {
            transform.DOLocalRotate(Vector3.up * 360, 10f)
                .SetLink(gameObject)
                .SetRelative()
                .SetEase(Ease.Linear)
                .SetId(this)
                .SetLoops(-1, LoopType.Incremental);
        }

        public void KillAnimations()
        {
            DOTween.Kill(this);
            transform.Reset();
        }
    }
}
