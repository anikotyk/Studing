using GameCore.Common.Controllers;
using Zenject;

#pragma warning disable 0649
namespace GameCore.Common.Installers
{
    public class GCAdsPreProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AdsEnableController>().AsSingle();
            Container.BindInterfacesTo<BannerAdCooldownController>().AsSingle();
            Container.BindInterfacesTo<InterstitialAdCooldownController>().AsSingle();
        }
    }
}