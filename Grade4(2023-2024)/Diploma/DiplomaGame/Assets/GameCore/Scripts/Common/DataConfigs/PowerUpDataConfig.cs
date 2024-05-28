using GameCore.Common.LevelItems.PowerUps;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Configs.NC;
using UnityEngine;

namespace GameCore.Common.DataConfigs
{
    [CreateAssetMenu(fileName = "PowerUpDC", menuName = CorePaths.DataConfigs + "/Power Up")]
    public class PowerUpDataConfig : DataConfig
    {
        public string title;
        public string description;
        public string shortDescription;
        
        public float intervalAppear;
        public float minDelayFirstAppear;
        public float maxDelayFirstAppear;
        public GameObject modelToWindow;
            
        public PowerUpContainer superPowerPrefab;
        
        public Sprite sprite;
    }
}