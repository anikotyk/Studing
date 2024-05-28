using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Configs.NC;
using UnityEngine;

namespace GameCore.GameScene.Helper
{
    [CreateAssetMenu(fileName = "HelperDataConfig", menuName = CorePaths.GameSettings + "/Data Configs/Helper")]
    public class HelperDataConfig : DataConfig
    {
        public string id;

        public int price;
    }
}