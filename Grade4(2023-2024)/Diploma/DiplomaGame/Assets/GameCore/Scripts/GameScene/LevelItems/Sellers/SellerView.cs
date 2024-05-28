using System.Collections;
using DG.Tweening;
using Dreamteck.Splines;
using GameCore.GameScene.DataConfigs;
using GameCore.GameScene.LevelItems.Managers;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.Settings;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Sellers
{
    public class SellerView : MonoBehaviour
    {
        [SerializeField] private SellerDataConfig _dataConfig;
        [SerializeField] private ProductsCarrier _carrier;
        [SerializeField] private GameObject _visual;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimatorParameterApplier _paddleAnim;
        [SerializeField] private AnimatorParameterApplier _notPaddleAnim;
        [SerializeField] private int _idleAnimsCount = 6;
        [SerializeField] private GameObject _paddle;
        [SerializeField] private ParticleSystem _vfxSwim;
        
        private static readonly int IdleSittingIndex = Animator.StringToHash("IdleSittingIndex");
        
        public string id => _dataConfig.id;
        
        private SplineComputer _splineComputer;
        
        private SplineFollower _splineFollower;
        public SplineFollower splineFollower
        {
            get
            {
                if (_splineFollower == null) _splineFollower = GetComponent<SplineFollower>();
                return _splineFollower;
            }
        }

        private Tween _scaleTween;
        private ProductsCarrier _carrierSell;

        private SellersManager _sellersManager;
        private bool _isInWayNow;
        public bool isInWayNow => _isInWayNow;

        private Coroutine _buyCoroutine;

        public readonly TheSignal onFinishWay = new();
        public readonly TheSignal onMiddleWay = new();
        
        private void Awake()
        {
            splineFollower.enabled = false;
            _visual.SetActive(false);
        }

        public void Initialize(SplineComputer splineComputer, ProductsCarrier carrier, SellersManager sellersManager)
        {
            _splineComputer = splineComputer;
            _carrierSell = carrier;
            _sellersManager = sellersManager;
            
            _splineComputer.triggerGroups[0].triggers[0].onUserCross += (user =>
            {
                if (user.gameObject == gameObject)
                {
                    OnSellingPoint();
                }
            });
            _splineComputer.triggerGroups[0].triggers[1].onUserCross += (user =>
            {
                if (user.gameObject == gameObject)
                {
                    OnEndPoint();
                }
            });
            
            splineFollower.spline = _splineComputer;
            splineFollower.enabled = true;
            splineFollower.Restart();
            DOVirtual.DelayedCall(0.01f, () =>
            {
                _visual.SetActive(true);
            },false).SetLink(gameObject);
            
        }

        public void StartMove(double startPoint = 0D)
        {
            if (_scaleTween != null)
            {
                _scaleTween.Kill();
            }
            _scaleTween = transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBack).SetLink(gameObject);
            splineFollower.Restart();
            _isInWayNow = true;
            splineFollower.follow = true;

            _vfxSwim.gameObject.SetActive(true);
            _vfxSwim.Play();
        }
        
        private void OnSellingPoint()
        {
            splineFollower.follow = false;
            _vfxSwim.Stop();
            SetRandomIdleSittingAnim();
            _paddle.SetActive(false);
            _notPaddleAnim.Apply();
            DOVirtual.DelayedCall(0.5f, () =>
            {
                BuyProducts();
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    onMiddleWay.Dispatch();
                    splineFollower.follow = true;
                    _paddle.SetActive(true);
                    _paddleAnim.Apply();
                    _vfxSwim.gameObject.SetActive(true);
                    _vfxSwim.Play();
                },false).SetLink(gameObject);
            },false).SetLink(gameObject);
            
        }

        private void BuyProducts()
        {
            if (!gameObject.activeInHierarchy) return;
            _buyCoroutine = StartCoroutine(BuyProductsCoroutine());
        }

        private IEnumerator BuyProductsCoroutine()
        {
            int cnt = Random.Range(GameplaySettings.def.sellersData.countTakeProducts / 3, GameplaySettings.def.sellersData.countTakeProducts);
            if (cnt > _carrierSell.products.Count)
            {
                cnt = _carrierSell.products.Count;
            }

            for (int i = 0; i < cnt; i++)
            {
                if(_carrierSell.products.Count <= 0) break;
                var product = _carrierSell.products[^1] as SellProduct;
                _carrierSell.GetOut(product);
                _carrier.Add(product, true);
                _sellersManager.GetMoney(product.priceDataConfig.softCurrencyCount);
                yield return new WaitForSeconds(0.075f);
            }
        }

        private void OnEndPoint()
        {
            _isInWayNow = false;
            splineFollower.follow = false;
            _carrier.ReleaseAllProducts();
            _vfxSwim.Stop();
            if (_scaleTween != null)
            {
                _scaleTween.Kill();
            }
            _scaleTween = transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
            }).SetLink(gameObject);
            onFinishWay.Dispatch();
        }

        private void SetRandomIdleSittingAnim()
        {
            _animator.SetFloat(IdleSittingIndex, Random.Range(0, _idleAnimsCount));
        }

        public void TurnOff()
        {
            if (_buyCoroutine != null)
            {
                StopCoroutine(_buyCoroutine);
            }
            _splineFollower.Move(1D);
            OnEndPoint();
        }
    }
}