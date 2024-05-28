using System;
using GameCore.Common.DataConfigs;
using GameCore.Common.LevelItems.Helper;
using GameCore.Common.LevelItems.PowerUps;
using GameCore.Common.UI.PowerUps;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Settings;
using UnityEngine;

namespace GameCore.Common.Settings
{
    public class PowerUpsSettings : Settings<PowerUpsSettings>
    {
        public float minDelayBetweenPowerUps = 60f;
        public bool isFirstPowerUpFree;
        public SuperPowerTimer superPowerTimer;
        
        public int pulseIconAfterAppearTimes = 3;
        public int disableWarningTime = 10;
        
        public int offerTime = 60;
        public int workingTime = 180;
        
        public PowerUpOffer powerUpOfferPrefab;
        
        public SuperPowerData capacitySuperPowerData;
        public SuperPowerData speedSuperPowerData;
        public SuperPowerData productionSuperPowerData;
        public SuperPowerData hittingSuperPowerData;
        
        public StackOfCashData stackOfCashData;
        
        public WateringSuperPowerData wateringSuperPowerData;
        
        [Serializable]
        public class SuperPowerData
        {
            public bool enabled;
            public bool isFirstFree;
            public PowerUpDataConfig config;
        }
        
        [Serializable]
        public class StackOfCashData
        {
            public bool enableStackOfCash;
            public bool isFirstFree;
            
            public float intervalAppear;
            public float minDelayFirstAppear;
            public float maxDelayFirstAppear;

            public Sprite icon;
            
            public StackOfCashContainer stackOfCashPrefab;
        }
        
        [Serializable]
        public class WateringSuperPowerData : SuperPowerData
        {
            public WateringCharacterView characterPrefab;
        }
    }
}