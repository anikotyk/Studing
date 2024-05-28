using GameBasicsCore.Game.Configs.DataConfigs.Groups;
using GameBasicsCore.Game.Configs.NC;
using UnityEngine;

namespace GameCore.ShipScene.DataConfigs
{
    [CreateAssetMenu(fileName = "WeaponsGroupDataConfig", menuName = CorePaths.GameSettings + "/Data Configs/Weapons Group")]
    public class WeaponsGroupDataConfig : GroupDataConfig<WeaponDataConfig>
    {
    }
}