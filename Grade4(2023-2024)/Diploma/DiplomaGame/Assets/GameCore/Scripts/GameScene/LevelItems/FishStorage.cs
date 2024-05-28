using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products.Storages;
using UnityEngine;

namespace GameCore.GameScene.LevelItems
{
    public class FishStorage : InjCoreMonoBehaviour
    {
        [SerializeField] private LimitedProductStorage _storage;
        public LimitedProductStorage storage => _storage;
    }
}