using System.Collections;
using GameBasicsSignals;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class InteractableProductionItem : ProductionItem
    {
        public readonly TheSignal onCanStartProduct = new();
        private int _cntToProduct;
        
        protected override void OnAddedObjectToPlatform()
        {
            if (!_isWorking && CanProduct())
            {
                onCanStartProduct.Dispatch();
            }
        }

        public override void StartWorking()
        {
            if(_isWorking) return;
            _isWorking = true;
            EffectOnStartWorking();
            StartCoroutine(UseAllProducts());
        }
        
        private IEnumerator UseAllProducts()
        {
            _cntToProduct = 0;
            while (CanProduct())
            {
                _cntToProduct++;
                yield return StartCoroutine(UseProducts());
            }
        }

        public override void EndWorking()
        {
            EffectOnEndWorking();
            for(int i = 0; i < _cntToProduct; i++) {
                SpawnProduct();
            }
            _isWorking = false;
            
            if (CanProduct())
            {
                onCanStartProduct.Dispatch();
            }
        }
    }
}