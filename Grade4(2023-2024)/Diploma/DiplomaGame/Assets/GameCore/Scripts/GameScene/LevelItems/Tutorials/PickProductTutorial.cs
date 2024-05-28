using GameCore.GameScene.LevelItems.Products;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Tutorials
{
    public class PickProductTutorial : RaftTutorial
    {
        [SerializeField] private SellProduct _product;
        public override Vector3 targetPos => _product.transform.position + Vector3.up * 0.5f;
        
        public override void Construct()
        {
            base.Construct();

            _product.onAddedToCarrier.Once(CompleteTutorial);
        }
        
        public override void StartTutorial()
        {
            base.StartTutorial();

            if (!_product.gameObject.activeSelf)
            {
                CompleteTutorial();
            }
        }
    }
}