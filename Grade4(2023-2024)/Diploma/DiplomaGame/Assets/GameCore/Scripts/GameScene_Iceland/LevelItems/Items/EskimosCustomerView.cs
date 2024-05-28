using DG.Tweening;
using GameCore.GameScene_Iceland.DataConfigs;
using GameCore.GameScene.Helper;
using GameCore.GameScene.LevelItems.Products;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene_Iceland.LevelItems.Items
{
    public class EskimosCustomerView : MonoBehaviour
    {
        [SerializeField] private EskimosCustomerDataConfig _dataConfig;
        [SerializeField] private Transform _visual;
        [SerializeField] private HelperAIPath _aiPath;
        [SerializeField] private AnimatorParameterApplier _idleAnim;
        [SerializeField] private AnimatorParameterApplier _moveAnim;
        
        private bool _isInWayNow;
        public bool isInWayNow => _isInWayNow;
        
        public string id => _dataConfig.id;

        private Tween _scaleTween;
        private ProductsCarrier _carrierSell;

        private Transform _startPoint;
        private Transform _sellPoint;
        private Transform _endPoint;
        
        public readonly TheSignal<SellProduct> onBoughtProduct = new();

        public void Initialize(ProductsCarrier carrierSell, Transform startPoint, Transform sellPoint, Transform endPoint)
        {
            _carrierSell = carrierSell;
            
            _startPoint = startPoint;
            _sellPoint = sellPoint;
            _endPoint = endPoint;
        }

        public void StartMove()
        {
            _isInWayNow = true;
            transform.position = _startPoint.position;
            
            if (_scaleTween != null)
            {
                _scaleTween.Kill();
            }

            _visual.localScale = Vector3.one * 0.01f;
            _scaleTween = _visual.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBack).SetLink(gameObject);
            
            _moveAnim.Apply();
            _aiPath.SetDestination(_sellPoint.position);
            _aiPath.onReachedDestination.Once(OnSellingPoint);
        }
        
        private void OnSellingPoint()
        {
            _idleAnim.Apply();
            DOVirtual.DelayedCall(0.5f, () =>
            {
                var product = GetSellProduct();
                //change clothes effect
                onBoughtProduct.Dispatch(product);
                product.Release();
                MoveToEndPoint();
            },false).SetLink(gameObject);
        }

        private SellProduct GetSellProduct()
        {
            if(_carrierSell.products.Count <= 0) return null;
            var product = _carrierSell.products[^1] as SellProduct;
            _carrierSell.GetOut(product);
            return product;
        }

        private void MoveToEndPoint()
        {
            _moveAnim.Apply();
            _aiPath.SetDestination(_endPoint.position);
            _aiPath.onReachedDestination.Once(OnEndPoint);
        }

        private void OnEndPoint()
        {
            _idleAnim.Apply();
            _isInWayNow = false;
            if (_scaleTween != null)
            {
                _scaleTween.Kill();
            }
            _scaleTween = _visual.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
            }).SetLink(gameObject);
        }
    }
}