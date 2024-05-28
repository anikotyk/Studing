using GameBasicsCore.Game.Models;
using UnityEngine;

namespace GameCore.GameScene.Helper
{
    public class HelperModel : CoreModel
    {
        public const uint Bought = 1;
        public const uint TurnedOn = 2;
        public const uint TurnedOff = 3;
        
        public string id => dataConfig.id;
        public int price => dataConfig.price;
        public bool isBought => _saveData.value.isBought;

        private bool _isTurnedOn = true;
        public bool isTurnedOn => _isTurnedOn;
        
        private readonly HelperSaveData _saveData;

        public HelperDataConfig dataConfig { get; }
        
        public HelperModel(HelperDataConfig dataConfig)
        {
            this.dataConfig = dataConfig;
            _saveData = new HelperSaveData();
        }
        
        public bool TryToBuy()
        {
            if (isBought)
            {
                Debug.LogError($"Helper is already bought.");
                return false;
            }
            
            BuyInternal();
            return true;
        }
        
        private void BuyInternal()
        {
            _saveData.value.isBought = true;
            DispatchChange(Bought);
        }

        public void Reset()
        {
            _saveData.Delete();
        }

        public void TurnOn()
        {
            _isTurnedOn = true;
            DispatchChange(TurnedOn);
        }
        
        public void TurnOff()
        {
            _isTurnedOn = false;
            DispatchChange(TurnedOff);
        }
    }
}