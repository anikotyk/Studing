using GameCore.ShipScene.Battle;
using GameCore.ShipScene.Battle.Waves;
using JetBrains.Annotations;
using Zenject;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine.Transitions
{
    public class VictoryTransition : EnemyStateTransition
    {
        [Inject, UsedImplicitly] public BattleController battleController { get; }
        protected override void OnTransitionStartListen()
        {
            if(battleController == null)
                return;
            battleController.ended.On(OnWaveEnded);
        }

        protected override void OnTransitionEndListen()
        {
            if(battleController == null)
                return;
            battleController.ended.Off(OnWaveEnded);
        }

        private void OnWaveEnded(Wave wave)
        {
            if(wave.waveStatus == WaveStatus.Failure)
                Transit();
        }
    }
}