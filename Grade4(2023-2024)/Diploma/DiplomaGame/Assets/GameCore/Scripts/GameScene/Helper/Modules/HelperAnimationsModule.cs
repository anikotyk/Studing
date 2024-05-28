using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using UnityEngine;

namespace GameCore.GameScene.Helper.Modules
{
    public class HelperAnimationsModule : CoreMonoBehaviour
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int LookAround = Animator.StringToHash("LookAround");
        private static readonly int Happy = Animator.StringToHash("Happy");
        private static readonly int Think = Animator.StringToHash("Think");
        private static readonly int HappyIndex = Animator.StringToHash("HappyIndex");

        [SerializeField] private int _happyAnimationsCount;

        private HelperView _viewCached;
        public HelperView view => _viewCached ??= GetComponentInParent<HelperView>();

        public void PlayIdle()
        {
            view.animator.SetTrigger(Idle);
        }

        public void PlayWalk()
        {
            view.animator.SetTrigger(Walk);
        }

        public void PlayLookAround()
        {
            view.animator.SetTrigger(LookAround);
        }

        public void PlayHappy()
        {
            view.animator.SetTrigger(Happy);
            var randomIndex = Random.Range(0, _happyAnimationsCount);
            view.animator.SetFloat(HappyIndex, randomIndex);
        }

        public void PlayThink()
        {
            view.animator.SetTrigger(Think);
        }

        public void ApplyAnimations(AnimatorParameterApplier _currentAnimation)
        {
            if (_currentAnimation == null) return;
            
            _currentAnimation.SetAnimator(view.animator);
            _currentAnimation.Apply();
        }
    }
}