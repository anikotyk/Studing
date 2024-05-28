using System;
using GameBasicsCore.Game.Misc;
using UnityEngine;

namespace GameCore.ShipScene.Weapons.Fx
{
    public class WeaponShootAnimationCaster : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private AnimatorParameterApplier _animatorParameterApplier;

        private Animator _animatorCached;

        private Animator animator
        {
            get
            {
                if (_animatorCached == null)
                    _animatorCached = GetComponentInParent<Animator>();
                return _animatorCached;
            }
        }

        private void OnEnable()
        {
            _weapon.shot.On(OnShot);
        }

        private void OnDisable()
        {
            _weapon.shot.Off(OnShot);
        }

        private void OnShot()
        {
            _animatorParameterApplier.SetAnimator(animator);
            _animatorParameterApplier.Apply();
        }
    }
}