using GameBasicsCore.Game.Configs.DataConfigs.Groups;
using GameBasicsCore.Game.Configs.NC;
using UnityEngine;

namespace GameCore.GameScene.DataConfigs
{
    [CreateAssetMenu(fileName = "SellersGroupDataConfig", menuName = CorePaths.GameSettings + "/Data Configs/Sellers Group")]
    public class SellersGroupDataConfig : GroupDataConfig<SellerDataConfig>
    {
        
    }
}