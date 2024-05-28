namespace GameCore.GameScene.LevelItems.Products
{
    public class SharkMeatProduct : SeaAnimatedProduct
    {
        public override void TurnOnInteractItem()
        {
            base.TurnOnInteractItem();
            StartSeaIdleAnim();
        }
        
        public override void TurnOffInteractItem()
        {
            base.TurnOffInteractItem();
            EndAnims();
        }
    }
}