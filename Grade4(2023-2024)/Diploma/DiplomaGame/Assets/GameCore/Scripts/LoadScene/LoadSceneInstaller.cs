using GameBasicsCore.Tools.Extensions;
using Zenject;

namespace GameCore.LoadScene
{
    public class LoadSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindDefault<LoadGameController>();
        }
    }
}