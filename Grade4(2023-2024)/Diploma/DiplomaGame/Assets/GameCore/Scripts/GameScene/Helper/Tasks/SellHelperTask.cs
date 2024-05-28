using System.Collections;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;

namespace GameCore.GameScene.Helper.Tasks
{
    public class SellHelperTask : HelperTask
    {
        private HelperView _view;
        private InteractorCharacterProductsCarrier _carrier;
        private ProductsCarrier _storageCarrier;
        private Transform _movePoint;
        
        protected override float maxTimeMoveToPoint => 40f;
        
        public void Initialize(HelperView view, InteractorCharacterProductsCarrier carrier, Transform movePoint, ProductsCarrier storageCarrier)
        {
            _view = view;
            _carrier = carrier;
            _storageCarrier = storageCarrier;
            _movePoint = movePoint;
        }

        protected override IEnumerator RunInternal()
        {
            if(_carrier.IsEmpty() && !_storageCarrier.HasSpace()) yield break;
            yield return null;
            _view.taskModule.MoveTo(_storageCarrier.name, _movePoint.position);
            
            yield return null;
            float timer = 0f;
            while (!_view.taskModule.aiPath.reachedDestination && !_carrier.IsEmpty() && _storageCarrier.HasSpace() && timer < maxTimeMoveToPoint)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            _view.locomotionMovingModule.StopMovement();
            while (!_carrier.IsEmpty() && _storageCarrier.HasSpace()) yield return null;
        }
    }
}