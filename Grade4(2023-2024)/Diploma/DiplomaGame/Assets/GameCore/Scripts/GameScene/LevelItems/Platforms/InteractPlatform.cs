using GameCore.GameScene.DataConfigs;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Platforms
{
    public class InteractPlatform : InjCoreMonoBehaviour
    {
        [SerializeField] private InteractPlatformIdDataConfig _idConfig;

        public string id => _idConfig.id;
    }
}