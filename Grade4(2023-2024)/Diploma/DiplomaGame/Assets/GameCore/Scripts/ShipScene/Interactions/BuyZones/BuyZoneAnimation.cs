using GameBasicsCore.Game.Core;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.ShipScene.Interactions
{
    public abstract class BuyZoneAnimation : InjCoreMonoBehaviour
    {
        [SerializeField] private CoinsBuyZone _buyZone;

        public CoinsBuyZone buyZone => _buyZone;
        public TheSignal<int> added { get; } = new();
        public TheSignal completed { get; } = new();
        
        private void OnEnable()
        {
            _buyZone.used.On(OnUsed);
        }

        private void OnDisable()
        {
            _buyZone.used.Off(OnUsed);
        }

        protected abstract void OnUsed(int usedAmount);
    }
}