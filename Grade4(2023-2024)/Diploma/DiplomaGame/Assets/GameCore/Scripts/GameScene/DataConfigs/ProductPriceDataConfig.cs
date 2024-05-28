using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Configs.NC;
using UnityEngine;

namespace GameCore.GameScene.DataConfigs
{
    [CreateAssetMenu(fileName = "ProductPriceDataConfig", menuName = CorePaths.GameSettings + "/Data Configs/Product Price")]
    public class ProductPriceDataConfig : DataConfig
    {
        public int softCurrencyCount;
    }
}