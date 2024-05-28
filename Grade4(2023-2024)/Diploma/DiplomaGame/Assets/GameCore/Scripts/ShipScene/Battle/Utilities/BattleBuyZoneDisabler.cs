using GameCore.ShipScene.Battle.Waves;
using GameCore.ShipScene.Interactions;
using UnityEngine;

namespace GameCore.ShipScene.Battle.Utilities
{
    public class BattleBuyZoneDisabler : BattleStartEndListener
    {
        [SerializeField] private CoinsBuyZone _buyZone;
        protected override void OnBattleStarted(Wave wave)
        {
            _buyZone.HideBuyPopUp();
        }

        protected override void OnBattleEnded(Wave wave)
        {
            if(_buyZone.gameObject.activeInHierarchy)
                _buyZone.ShowBuyPopUp();
        }
    }
}