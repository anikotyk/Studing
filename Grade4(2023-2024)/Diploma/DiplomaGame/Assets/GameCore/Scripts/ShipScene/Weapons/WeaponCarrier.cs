using System;
using GameCore.ShipScene.Battle;
using GameCore.ShipScene.Battle.Utilities;
using GameCore.ShipScene.Battle.Waves;
using StaserSDK.Animations;
using UnityEngine;

namespace GameCore.ShipScene.Weapons
{
    public class WeaponCarrier : BattleStartEndListener
    {
        [SerializeField] private WeaponLinker _weaponLinker;
        [SerializeField] private AnimatorLinker _animatorLinker;
        
        private void OnDisable()
        {
            RevertAll();
        }

        protected override void OnBattleStarted(Wave wave)
        {
            RevertAll();
            ApplyAnimation(_weaponLinker.weapon);
        }

        protected override void OnBattleEnded(Wave wave)
        {
            RevertAll();
        }

        private void RevertAll()
        {
            _weaponLinker.weapons.ForEach(RevertAnimation);
        }
        
        private void ApplyAnimation(Weapon weapon)
        {
            weapon.handleAnimation.SetAnimator(_animatorLinker.animator);
            weapon.handleAnimation.Apply();
        }

        private void RevertAnimation(Weapon weapon)
        {
            weapon.handleAnimation.SetAnimator(_animatorLinker.animator);
            weapon.handleAnimation.Revert();
        }
    }
}