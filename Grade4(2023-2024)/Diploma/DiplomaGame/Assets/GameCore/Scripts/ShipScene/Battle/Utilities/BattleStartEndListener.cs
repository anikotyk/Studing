using GameCore.ShipScene.Battle.Waves;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using Zenject;

namespace GameCore.ShipScene.Battle.Utilities
{
    public abstract class BattleStartEndListener : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public BattleController battleController { get; }

        public override void Construct()
        {
            battleController.started.On(OnBattleStarted);
            battleController.ended.On(OnBattleEnded);
        }

        protected abstract void OnBattleStarted(Wave wave);

        protected abstract void OnBattleEnded(Wave wave);
    }
}