using DG.Tweening;
using UnityEngine;

#pragma warning disable 0649
namespace GameCore.Common.Misc
{
    public static class GCAnimations
    {
        public static (Tweener tweenButton, Tweener tweenLabel)
            PlayPulseScaleButtonAndLabelAnimation(Transform button, Transform label)
        {
            var tweenButton = PlayPulse(button);
            var tweenLabel = label.transform.DOScale(1.05f, 0.4f)
                .SetDelay(0.2f)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
            return (tweenButton, tweenLabel);
        }

        public static Tweener PlayPulse(Transform transform)
        {
            var tween = transform.DOScale(1.05f, 0.4f)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
            return tween;
        }
    }
}