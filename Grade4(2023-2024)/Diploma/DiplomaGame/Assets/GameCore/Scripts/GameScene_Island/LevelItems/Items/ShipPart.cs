using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.GameScene.Audio;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Tools.DOTweenAnimations;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class ShipPart : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public ShipCraft shipCraft { get; }
        [Inject, UsedImplicitly] public PopSoundManager popSoundManager { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        
        [SerializeField] private BuyObject _buyObjectToActivate;
        [SerializeField] private Transform _shipStage;

        private InteractItem _interactItemCached;
        public InteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<InteractItem>();
                return _interactItemCached;
            }
        }
        
        private bool _isUsed;
        
        private bool _isActive;
        public bool isActive => _isActive;
        
        private Tween _pulseTween;
        
        public TheSignal onActivate { get; } = new();
        public TheSignal onTaken { get; } = new();

        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 10000);
        }

        private void Initialize()
        {
            _isActive = true;
            
            if (shipCraft.IsStageActive(_shipStage))
            {
                _isUsed = true;
                DeactivateInternal();
            }
            else if (!_buyObjectToActivate.isBought)
            {
                DeactivateInternal();
                _buyObjectToActivate.onBuy.Once(() =>
                {
                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        Activate();
                    }, false).SetLink(gameObject);
                });
            }
        }
        
        public void OnTaken()
        {
            interactItem.enabled = false;

            if (_isUsed)
            {
                gameObject.SetActive(false);
                return;
            }
            
            _isUsed = true;
           
            popSoundManager.PlaySound();
            if (_pulseTween != null)
            {
                _pulseTween.Kill();
            }
            
            transform.DOScale(Vector3.one * 0.01f, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
            {
                shipCraft.ActivateShipStage(_shipStage);
                onTaken.Dispatch();
                gameObject.SetActive(false);
            }).SetLink(gameObject);
        }

        private void DeactivateInternal()
        {
            _isActive = false;
            gameObject.SetActive(false);
            
            if (_pulseTween != null)
            {
                _pulseTween.Kill();
            }
        }

        private void Activate()
        {
            _isActive = true;
            onActivate.Dispatch();
            gameObject.SetActive(true);
            transform.localScale = Vector3.one * 0.01f;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                _pulseTween = transform.DOPulseScaleDefault(1f, 1.025f, 0.5f)
                    .SetLink(gameObject)
                    .SetLoops(-1);
            }).SetLink(gameObject);
        }
    }
}