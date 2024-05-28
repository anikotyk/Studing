using System;
using GameCore.Common.LevelItems.PowerUps;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Configs.DataConfigs.Groups;
using GameBasicsCore.Game.Settings;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;

namespace GameCore.GameScene.Settings
{
    public class GameplaySettings : Settings<GameplaySettings>
    {
        public UpgradesGroupDataConfig upgradesGroup;
        public VirtualCameraIdDataConfig wateringVCamIdConfig;
        public VirtualCameraIdDataConfig appleHittingVCamIdConfig;
        public VirtualCameraIdDataConfig millGrindingVCamIdConfig;
        public VirtualCameraIdDataConfig shipVCamIdConfig;
        public VirtualCameraIdDataConfig helperVCamIdConfig;
        public VirtualCameraIdDataConfig tileInBuyModeVCamIdConfig;
        public VirtualCameraIdDataConfig helperAppearVCamIdConfig;
        public VirtualCameraIdDataConfig helperSonAppearVCamIdConfig;
        public VirtualCameraIdDataConfig helperTribeManAppearVCamIdConfig;
        public VirtualCameraIdDataConfig stallVCamIdConfig;
        public VirtualCameraIdDataConfig workbenchVCamIdConfig;
        public ProductsGroupDataConfig stackProducts;

        public float raftsMoveDownUnderCharacterYPos = -0.075f;
        public float tribeManWorkingTime = 40f;

        public SpawnWoodData spawnWoodData;

        [Serializable]
        public class SpawnWoodData
        {
            public float spawnWoodInterval = 5f;
            public int spawnAtStartCount = 10;
            public int maxSpawnedWoodCount = 50;
            
            public float spawnPosRange = 10;
        }
        
        public WateringData wateringData;
        
        [Serializable]
        public class WateringData
        {
            public float wateringTime = 5f;
            public float timeNeedHoldTouch = 3f;
            public float notAvailableTime = 5f;
        }
        
        public FishingData fishingData;
        
        [Serializable]
        public class FishingData
        {
            public float intervalFishing = 30f;
            public int netFishesCount = 5;
        }
        
        public SellersData sellersData;
        
        [Serializable]
        public class SellersData
        {
            public float intervalSpawnSellers = 5f;
            public int countTakeProducts = 5;
        }
        
        public JumpFishData jumpFishData;
        
        [Serializable]
        public class JumpFishData
        {
            public float intervalMin = 5f;
            public float intervalMax = 10f;
        }
        
        public SharksData sharksData;
        
        [Serializable]
        public class SharksData
        {
            public int countMeatFromHitMax = 2;
            public int countHits = 3;
            public float angleCanHit = 75f;
            public float rangeJumpMeat = 0.5f;
            public float intervalMeatShark = 60f;
        }
        
        public FruitTreeData fruitTreeData;
        
        [Serializable]
        public class FruitTreeData
        {
            public float respawnTime = 120f;
        }
    }
}