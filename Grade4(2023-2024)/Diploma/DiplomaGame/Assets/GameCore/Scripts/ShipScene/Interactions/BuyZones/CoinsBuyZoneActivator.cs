using System;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.ShipScene.Common;
using GameBasicsCore.Game.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore.ShipScene.Interactions
{
    public class CoinsBuyZoneActivator : InjCheckCoreMonoBehaviour
    {
        [SerializeField] private CoinsBuyZone _coinsBuyZone;
        [SerializeField] private List<GameObject> _goToDisable;
        [SerializeField] private List<GameObject> _goToEnable;
        [SerializeField] private List<Transform> _objectsToZoomOut;
        [SerializeField] private float _zoomDuration;
        [SerializeField] private float _buyDelay;
        [SerializeField] private float _activationDelay;
        
        private void OnEnable()
        {
            if(IsInjected)
                Validate();
            _coinsBuyZone.bought.On(OnBought);
        }

        private void OnDisable()
        {
            _coinsBuyZone.bought.Off(OnBought);
        }

        public override void Construct()
        {
            base.Construct();
            Validate(true);
        }

        private void Validate(bool skipDelay = false)
        {
            if (_coinsBuyZone.IsBought())
            {
                OnBought(skipDelay);
                return;
            }
            _goToEnable.ForEach(x=>x.SetActive(false));
        }

        private void OnBought() => DOVirtual.DelayedCall(_buyDelay, () => OnBought(false));
        
        private void OnBought(bool skipDelay)
        {
            _objectsToZoomOut.ForEach(x=>
                x.DOScale(0, _zoomDuration).SetEase(Ease.InBack).OnComplete(() =>
                {
                    x.gameObject.SetActive(false);
                }));
            for (int i = 0; i < _goToEnable.Count; i++)
            {
                int index = i;
                if(skipDelay == false)
                    DOVirtual.DelayedCall(i * _activationDelay, () => _goToEnable[index].SetActive(true));
                else
                    _goToEnable[index].SetActive(true);
            }
            _goToDisable.ForEach(x=>x.SetActive(false));
        }
    }
}