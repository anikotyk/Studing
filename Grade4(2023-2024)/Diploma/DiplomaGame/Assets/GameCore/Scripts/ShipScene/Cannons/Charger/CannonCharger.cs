using System;
using DG.Tweening;
using GameCore.ShipScene.Common;
using GameBasicsCore.Game.Views;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.Misc;
using StaserSDK.Extentions;
using StaserSDK.Interactable;
using UnityEngine;

namespace GameCore.ShipScene.Cannons
{
    public class CannonCharger : MaxView
    {
        [SerializeField] private Cannon _cannon;
        [SerializeField] private Transform _chargePoint;
        [SerializeField] private float _jumpPower;
        [SerializeField] private float _moveDuration;
        [SerializeField] private ProductDataConfig _kernelConfig;
        [SerializeField] private ZoneBase _zone;

        private int _chargeDelayed = 0;
        
        private void OnEnable()
        {
            _zone.onEnter.On(OnEnter);
            _zone.onInteract.On(OnInteract);
        }

        private void OnDisable()
        {
            _zone.onEnter.Off(OnEnter);
            _zone.onInteract.Off(OnInteract);
        }

        private void OnEnter(InteractableCharacter character)
        {
            if(_cannon.isFull == false)
                return;
            if(character.productsCarrier.products.Count == 0)
                return;
            Max(character.transform.position);
        }
        
        private void OnInteract(InteractableCharacter character)
        {
            if(CanAdd(character) == false)
                return;
            
            var kernel = character.productsCarrier.GetOutLast(_kernelConfig);
            
            _chargeDelayed++;
            
            kernel.transform.DOJump(_chargePoint.position, _jumpPower, 1, _moveDuration)
                .OnComplete(()=>
                {
                    kernel.Release();
                    _cannon.AddChange();
                    _chargeDelayed--;
                    if(_cannon.isFull)
                        Max(character.transform.position);
                });
                
        }

        private bool CanAdd(InteractableCharacter character)
        {
            if(_cannon.isFull)
                return false;
            
            if(_cannon.currentChargeCount + _chargeDelayed >= _cannon.capacity)
                return false;
            
            if(character.productsCarrier.products.Count == 0)
                return false;

            if (character.productsCarrier.Has(_kernelConfig) == false)
                return false;

            return true;
        }
    }
}