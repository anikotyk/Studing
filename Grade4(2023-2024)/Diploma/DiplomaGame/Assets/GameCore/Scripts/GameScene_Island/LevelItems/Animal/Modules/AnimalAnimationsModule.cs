using GameCore.Common.LevelItems.Animals;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Modules
{
    public class AnimalAnimationsModule : CoreMonoBehaviour
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Eat = Animator.StringToHash("Eat");
        private static readonly int GetProduct = Animator.StringToHash("GetProduct");
        
        private AnimalView _viewCached;
        public AnimalView view => _viewCached ??= GetComponentInParent<AnimalView>();

        public void PlayIdle()
        {
            view.animator.SetTrigger(Idle);
        }

        public void PlayWalk()
        {
            view.animator.SetTrigger(Walk);
        }
        
        public void PlayGetProduct()
        {
            view.animator.SetTrigger(GetProduct);
        }
        
        public void PlayEat()
        {
            view.animator.SetTrigger(Eat);
        }

        public void ApplyAnimations(AnimatorParameterApplier _currentAnimation)
        {
            if (_currentAnimation == null) return;
            
            _currentAnimation.SetAnimator(view.animator);
            _currentAnimation.Apply();
        }
    }
}