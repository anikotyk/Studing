using GameCore.Common.Controllers.PowerUps;
using GameCore.Common.Settings;
using UnityEngine;

namespace GameCore.Common.UI.PowerUps
{
    public class SuperPowerTimersContainer : MonoBehaviour
    {
        public void SpawnTimer(ITimerablePowerUpController controller, Sprite sprite)
        {
            var superPowerTimer = Instantiate(PowerUpsSettings.def.superPowerTimer ,transform);
            superPowerTimer.Init(controller, sprite);
        }
    }
}
