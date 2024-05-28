using GameCore.Common.LevelItems.Managers;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.PowerUps
{
    public class PowerUpPoint : MonoBehaviour
    {
        [InjectOptional, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        private BuyObject _buyObjectCached;
        public BuyObject buyObject
        {
            get
            {
                if (_buyObjectCached == null) _buyObjectCached  = GetComponentInParent<BuyObject>(true);
                return _buyObjectCached;
            }
        }

        public bool IsAvailable()
        {
            return (!buyObject || buyObject.isBought) && !IsCloseToCurrentBuyObject();
        }
        
        private bool IsCloseToCurrentBuyObject()
        {
            if (buyObjectsManager.currentBuyObject)
            {
                if (buyObjectsManager.currentBuyObject is DonateBuyObject donateBuyObject)
                {
                    Vector3 pos = donateBuyObject.buyPlatform.transform.position;
                    
                    if (Vector3.Distance(pos, transform.position) < 0.5f)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}