using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Interactions
{
    public abstract class ProductLimiter : InjCoreMonoBehaviour
    {
        [SerializeField] private ProductDataConfig _config;

        [Inject, UsedImplicitly] public ProductLimitManager productLimitManager { get; }
        
        public abstract bool isAvailable { get; }
        public abstract int limit { get; }
        public abstract int freeSpace { get; }
        
        public bool isFull => freeSpace > 0;
        public ProductDataConfig config => _config;

        public override void Construct()
        {
            productLimitManager.AddLimiter(this);
        }
    }
}