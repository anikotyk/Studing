using System;
using DG.Tweening;
using UnityEngine;

namespace GameCore.CutScenes.Animations
{
    public abstract class CutSceneAnimationItem : MonoBehaviour
    {
        private void OnDisable()
        {
            DOTween.Kill(this);
        }

        protected abstract void PrepareInternal();
        protected abstract Tween AnimateInternal();
        
        public Tween Animate()
        {
            PrepareInternal();
            return AnimateInternal().SetId(this).SetUpdate(false);
        }
    }
}