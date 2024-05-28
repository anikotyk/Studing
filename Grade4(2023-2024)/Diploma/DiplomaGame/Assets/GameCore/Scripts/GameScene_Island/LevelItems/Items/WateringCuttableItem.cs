namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class WateringCuttableItem : CuttableItem
    {
        private bool _isUnwatered;
        
        public void OnWatered()
        {
            if (_isUnwatered)
            {
                StartRespawn();
            }
            _isUnwatered = false;
        }
        
        public void OnWateredAndRespawned()
        {
            _isUnwatered = false;
            RespawnInternal();
        }

        protected override void OnCutted()
        {
            _isUnwatered = true;
            onReadyToRespawn.Dispatch();
        }
    }
}