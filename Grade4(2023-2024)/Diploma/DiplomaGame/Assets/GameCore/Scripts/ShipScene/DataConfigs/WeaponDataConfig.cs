using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Configs.NC;
using UnityEngine;

namespace GameCore.ShipScene.DataConfigs
{
    [CreateAssetMenu(fileName = "WeaponDataConfig", menuName = CorePaths.GameSettings + "/Data Configs/Weapon")]
    public class WeaponDataConfig : DataConfig
    {
        public string name;
        public Sprite icon;
    }
}