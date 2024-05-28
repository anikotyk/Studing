using GameCore.ShipScene.Currency;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Settings.GameCore.Modules.GameCurrency;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Battle.Waves
{
    public class WaveFinishReward : InjCoreMonoBehaviour
    {
        [SerializeField] private Wave _wave;
        [SerializeField] private int _reward;

        [Inject] private ShipCoinsCollectModel _shipCoinsCollectModel;
        private GameCurrencyGCSModule settings => ShipSceneSettings.def.currencyModule;

        public override void Construct()
        {
            _wave.finished.On(ApplyReward);
        }
        
        private void ApplyReward()
        {
            _shipCoinsCollectModel.Earn(_reward, settings.currencyName, settings.currencyName);
        }

    }
}