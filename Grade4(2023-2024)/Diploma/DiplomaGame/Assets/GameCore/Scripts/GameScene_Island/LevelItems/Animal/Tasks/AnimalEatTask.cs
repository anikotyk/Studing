using System.Collections;
using DG.Tweening;
using GameCore.GameScene_Island.LevelItems.Platforms;
using GameCore.GameScene.Helper.Tasks;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Tasks
{
    public class AnimalEatTask : HelperTask
    {
        private AnimalProductingView _view;
        private FeedPlatform _feedPlatform;
        private ProductDataConfig _productDataConfig;

        public void Initialize(AnimalProductingView view, FeedPlatform feedPlatform, ProductDataConfig productDataConfig)
        {
            _view = view;
            _feedPlatform = feedPlatform;
            _productDataConfig = productDataConfig;
        }
        
        protected override IEnumerator RunInternal()
        {
            _view.locomotionMovingModule.StartMovement();
            _view.taskModule.MoveTo(_feedPlatform.name, _feedPlatform.eatPoint.position);
            yield return null;
            float timer = 0;
            while (!_view.taskModule.aiPath.reachedDestination && isRunning && timer < maxTimeMoveToPoint)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            yield return null;
            _view.transform.DORotate(_feedPlatform.eatPoint.rotation.eulerAngles, 0.5f).SetLink(_view.gameObject);
            _view.transform.DOMove(_feedPlatform.eatPoint.position, 0.5f).SetLink(_view.gameObject);
            yield return new WaitForSeconds(0.5f);
            
            _view.locomotionMovingModule.StopMovement();

            if (_feedPlatform.productsCarrier.products.Count > 0)
            {
                _feedPlatform.TurnOffInteractItem();
            
                while (_feedPlatform.productsCarrier.products.Count > 0)
                {
                    var product = _feedPlatform.productsCarrier.GetOutLast(_productDataConfig);
                    product.transform.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        product.gameObject.SetActive(false);
                    }).SetLink(product.gameObject);
                
                    _view.animationsModule.PlayEat();
                
                    yield return new WaitForSeconds(0.5f);
                }
            
                _view.productionModule.Eat();
                _feedPlatform.TurnOnInteractItem();
            }
        }
    }
}