using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems
{
    public class BuyObject : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [SerializeField] private string _objectName = "tile";
        public string objectName => _objectName;
        [SerializeField] private string _actionName = "Build";
        public string actionName => _actionName;
        
        private bool _isBought = false;
        public bool isBought => _isBought;
        
        private bool _isInBuyMode = false;
        public bool isInBuyMode => _isInBuyMode;
        
        public readonly TheSignal onBuy = new();
        public readonly TheSignal onSetInBuyMode = new();

        public virtual void ActivateInternal()
        {
            _isInBuyMode = false;
            _isBought = true;
        }
        
        public virtual void DeactivateInternal()
        {
            _isInBuyMode = false;
        }
        
        public virtual void SetInBuyModeInternal(bool isToUseSaves = false)
        {
            _isInBuyMode = true;
        }
        
        public virtual void SetInBuyMode()
        {
            _isInBuyMode = true;
            onSetInBuyMode.Dispatch();
        }
        
        public virtual void Buy()
        {
            if (_isBought) return;
            _isBought = true;
            _isInBuyMode = false;

            DOVirtual.DelayedCall(0.5f, () =>
            {
                buyObjectsManager.ActivateNext();
            }, false).SetLink(gameObject);
            
            onBuy.Dispatch();
        }
    }
}