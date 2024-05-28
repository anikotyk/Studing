using GameCore.Common.LevelItems;
using GameBasicsCore.Game.Core;

namespace GameCore.Common.Misc
{
    public class ActionAfterBuy : InjCoreMonoBehaviour
    {
        private BuyObject _buyObjectCached;
        public BuyObject buyObject
        {
            get
            {
                if (_buyObjectCached == null) _buyObjectCached = GetComponent<BuyObject>();
                return _buyObjectCached;
            }
        }
        
        public override void Construct()
        {
            base.Construct();
            buyObject.onBuy.Once(Action);
        }

        protected virtual void Action()
        {
            
        }
    }
}