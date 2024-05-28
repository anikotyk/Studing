using DG.Tweening;
using EPOOutline;
using GameCore.Common.LevelItems;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSignals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class OldFilter : ProductView
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [SerializeField] private BuyObject _waterFilterBuyObject;
        
        private InteractItem _interactItemCached;
        public InteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<InteractItem>();
                return _interactItemCached;
            }
        }
        
        private Outlinable _outlinableCached;
        public Outlinable outlinable
        {
            get
            {
                if (_outlinableCached == null) _outlinableCached = GetComponent<Outlinable>();
                return _outlinableCached;
            }
        }
        
        private Tween _rotateTween;
        private Tween _moveYTween;
        private float _timeRotateAroundYAxis = 60f;
        private float _timeMoveYAxis = 30f;

        public readonly TheSignal onTaken = new();
        public readonly TheSignal onReadyToTake = new();
        private TheSaveProperty<bool> _isOldFilterTaken;
        
        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 10000);
        }

        private void Initialize()
        {
            _isOldFilterTaken = new(CommStr.OldFilterTaken);

            if (_isOldFilterTaken.value || _waterFilterBuyObject.isBought)
            {
                SetNotAvailable();
                gameObject.SetActive(false);
            }
            else
            {
                StartSeaIdleAnim();
                if (_waterFilterBuyObject.isInBuyMode)
                {
                    SetAvailable();
                }
                else
                {
                    SetNotAvailable();
                    _waterFilterBuyObject.onSetInBuyMode.Once(SetAvailable);
                }
                
                _waterFilterBuyObject.onBuy.Once(() =>
                {
                    if (!_isOldFilterTaken.value)
                    {
                        OnInteracted();
                    }
                });
            }
        }

        private void StartSeaIdleAnim()
        {
            StartSeaIdleAnimWithoutMove();
        }
        
        private void StartSeaIdleAnimWithoutMove()
        {
            int side = Random.Range(0, 2);
            side = side > 0 ? 1 : -1;
            _rotateTween = transform
                .DOLocalRotate(new Vector3(0, 360 * side, 0), _timeRotateAroundYAxis, RotateMode.FastBeyond360)
                .SetRelative(true)
                .SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental).SetLink(gameObject);
            _moveYTween = transform.DOLocalMoveY(0.05f, _timeMoveYAxis).SetRelative(true)
                .SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        }
        
        public void EndAnims()
        {
            if (TryGetComponent<Collider>(out Collider collider))
            {
                collider.enabled = false;
            }

            if (_rotateTween != null)
            {
                _rotateTween.Kill();
            }
            
            if (_moveYTween != null)
            {
                _moveYTween.Kill();
            }
        }

        public void OnInteracted()
        {
            _isOldFilterTaken.value = true;
            onTaken.Dispatch();

            transform.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
            }).SetLink(gameObject);
        }

        private void SetAvailable()
        {
            onReadyToTake.Dispatch();
            interactItem.enabled = true;
            outlinable.enabled = true;
        }
        
        private void SetNotAvailable()
        {
            interactItem.enabled = false;
            outlinable.enabled = false;
        }
    }
}