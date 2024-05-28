using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Enemy>().FromComponentOn(gameObject).AsSingle();
        }
    }
}