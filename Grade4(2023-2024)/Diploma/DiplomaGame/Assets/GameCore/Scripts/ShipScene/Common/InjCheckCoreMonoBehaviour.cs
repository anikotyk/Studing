using GameBasicsCore.Game.Core;

namespace GameCore.ShipScene.Common
{
    public class InjCheckCoreMonoBehaviour : InjCoreMonoBehaviour
    {
        public bool IsInjected { get; private set; } = false;
        
        public override void Construct()
        {
            IsInjected = true;
        }
    }
}