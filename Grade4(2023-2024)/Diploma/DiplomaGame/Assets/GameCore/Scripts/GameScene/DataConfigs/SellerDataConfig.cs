using GameCore.GameScene.LevelItems.Sellers;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Configs.NC;
using UnityEngine;

namespace GameCore.GameScene.DataConfigs
{
    [CreateAssetMenu(fileName = "SellerDataConfig", menuName = CorePaths.GameSettings + "/Data Configs/Seller")]
    public class SellerDataConfig : DataConfig
    {
        public string id;
        public SellerView viewPrefab;
    }
}