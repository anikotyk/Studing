using GameCore.Common.LevelItems;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class WhileCantFly : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }

        [SerializeField] private GameObject[] _borders;
        [SerializeField] private GameObject _fakeElevator;
        [SerializeField] private GameObject _realElevator;
        [SerializeField] private BuyObject _buyObjectToDisable;

        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 10000);
        }
        
        private void Initialize()
        {
            if (_buyObjectToDisable.isBought)
            {
                OnCanFly();
            }
            else
            {
                OnCantFly();
                _buyObjectToDisable.onBuy.Once(() =>
                {
                    OnCanFly();
                });
            }
        }

        private void OnCantFly()
        {
            _fakeElevator.SetActive(true);
            _realElevator.SetActive(false);
            EnableBorders();
        }
        
        private void OnCanFly()
        {
            _fakeElevator.SetActive(false);
            _realElevator.SetActive(true);
            DisableBorders();
        }

        private void DisableBorders()
        {
            foreach (var border in _borders)
            {
                border.SetActive(false);
            }
        }
        
        private void EnableBorders()
        {
            foreach (var border in _borders)
            {
                border.SetActive(true);
            }
        }
    }
}