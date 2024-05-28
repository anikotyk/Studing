using UnityEngine;

namespace GameCore.Common.LevelItems.Managers
{
    public class BuyLevelEventsManager : LevelEventsManager
    {
        [SerializeField] private BuyObject _startLevelBuyObject;
        [SerializeField] private BuyObject _completeLevelBuyObject;
        
        public override bool isActive => _startLevelBuyObject.isBought;
        
        public override void Construct()
        {
            base.Construct();

            _startLevelBuyObject.onBuy.Once(OnLevelStart);
            _completeLevelBuyObject.onBuy.Once(OnLevelComplete);
        }

        public void OnBuyObject(string stepName)
        {
            OnLevelStepState(EventState.Complete, stepName);
        }
    }
}