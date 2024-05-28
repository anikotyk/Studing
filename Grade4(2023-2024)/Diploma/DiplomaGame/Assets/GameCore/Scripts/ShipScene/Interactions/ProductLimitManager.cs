using System.Collections.Generic;
using System.Linq;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;

namespace GameCore.ShipScene.Interactions
{
    public class ProductLimitManager
    {
        private List<ProductLimiter> _limiters { get; } = new();

        public ProductLimitManager(){}

        public void AddLimiter(ProductLimiter productLimiter)
        {
            if(_limiters.Contains(productLimiter))
                return;
            _limiters.Add(productLimiter);
        }

        public bool HasLimitation(ProductDataConfig config)
        {
            return _limiters.Has(x => x.config.id == config.id);
        }

        public int GetLimit(ProductDataConfig config)
        {
            if (HasLimitation(config) == false)
                return int.MaxValue;
            return _limiters.Where(x=>x.isAvailable).Sum(x => x.freeSpace);
        }
    }
}