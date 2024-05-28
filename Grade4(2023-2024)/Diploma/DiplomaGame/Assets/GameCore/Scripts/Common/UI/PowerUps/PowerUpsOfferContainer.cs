using GameCore.Common.Controllers.PowerUps;
using GameCore.Common.Settings;
using UnityEngine;

namespace GameCore.Common.UI.PowerUps
{
    public class PowerUpsOfferContainer : MonoBehaviour
    {
        public PowerUpOffer SpawnOffer(PowerUpController controller)
        {
            var powerUpOffer = Instantiate(PowerUpsSettings.def.powerUpOfferPrefab ,transform);
            powerUpOffer.Init(controller);
            return powerUpOffer;
        }
    }
}