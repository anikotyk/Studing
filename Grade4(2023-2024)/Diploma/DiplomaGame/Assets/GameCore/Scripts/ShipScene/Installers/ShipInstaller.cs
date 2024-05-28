using GameCore.ShipScene.Currency;
using GameCore.ShipScene.Interactions;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Installers
{
    public class ShipInstaller : MonoInstaller
    {
        [SerializeField] private ProductsCarrier _shipCoinsProductCarrier;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShipCoinsCollectModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ShipCoinsCurrencyPresenter>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<ProductLimitManager>().AsSingle().NonLazy();
        }
    }
}