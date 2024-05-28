using GameCore.GameScene_Iceland.LevelItems.Items;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Configs.NC;
using UnityEngine;

namespace GameCore.GameScene_Iceland.DataConfigs
{
    [CreateAssetMenu(fileName = "EskimosCustomerDataConfig", menuName = CorePaths.GameSettings + "/Data Configs/Eskimos Customer")]
    public class EskimosCustomerDataConfig : DataConfig
    {
        public string id;
        public EskimosCustomerView view;
    }
}