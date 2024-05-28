using GameBasicsCore.Game.Settings;
using UnityEngine;

namespace GameCore.ShipScene.Currency
{
    [CreateAssetMenu]
    public class ShipSceneSettings : Settings<ShipSceneSettings>
    {
        public ShipCurrencyModule currencyModule;
    }
}