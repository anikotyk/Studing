using System.Collections;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products.Storages;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;

namespace GameCore.GameScene.Helper.Tasks
{
    public class GetProductFromStorageHelperTask : HelperTask
    {
        private HelperView _view;
        private InteractorCharacterProductsCarrier _carrier;
        private LimitedProductStorage _storage;
        public LimitedProductStorage storage => _storage;

        public void Initialize(HelperView view, InteractorCharacterProductsCarrier carrier, LimitedProductStorage storage)
        {
            _view = view;
            _carrier = carrier;
            _storage = storage;
        }

        protected override IEnumerator RunInternal()
        {
            if(!storage.Has()) yield break;
            yield return null;
            _view.taskModule.MoveTo(_storage.name, _storage.transform.position);
            yield return null;
            float timer = 0;
            while (!_view.taskModule.aiPath.reachedDestination && _carrier.HasSpace() && _storage.Has() && timer < maxTimeMoveToPoint)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            _view.locomotionMovingModule.StopMovement();
            while (_carrier.HasSpace() && _storage.Has()) yield return null;
        }
    }
}