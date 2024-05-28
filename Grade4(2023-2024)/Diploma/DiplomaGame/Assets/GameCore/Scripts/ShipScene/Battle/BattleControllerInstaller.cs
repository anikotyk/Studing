using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Battle
{
    public class BattleControllerInstaller : MonoInstaller
    {
        [SerializeField] private BattleController _battleController;
        public override void InstallBindings()
        {
            Container.Bind<BattleController>().FromInstance(_battleController);
        }
    }
}