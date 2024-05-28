using System;
using DG.Tweening;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using StaserSDK.Extentions;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Weapons.Bullets.Movement
{
    public class MainCharacterWeaponShootingRotatator : WeaponIsShootingListener
    {
        [SerializeField] private float _rotationSpeed;
        
        [Inject, UsedImplicitly] public MainCharacterSimpleWalkerController walker { get; }

        private float _startRotationSpeed = 10f;

        private Transform _target;
        
        public override void Construct()
        {
            base.Construct();
            _startRotationSpeed = walker.rotationSpeed;
        }

        protected override void OnShootingStarted(Transform target)
        {
            if(IsInjected == false)
                walker.rotationSpeed = 0;
            _target = target;
        }

        protected override  void OnShootingEnded()
        {
            if(IsInjected)
                walker.rotationSpeed = _startRotationSpeed;
            _target = null;
        }

        private void Update()
        {
            if(isShooting == false)
                return;
            
            var characterTransform = walker.transform;
            var relativePosition = _target.position - characterTransform.position;

            var targetRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
            characterTransform.rotation = targetRotation.Multiply(Vector3.up);
        }
    }
}