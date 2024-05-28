namespace GameCore.ShipScene.Enemies.EnemyStateMachine.Transitions
{
    public class DeathTransition : EnemyStateTransition
    {
        protected override void OnTransitionStartListen()
        {
            if (enemy.health.isDied)
            {
                Transit();
                return;
            }

            enemy.health.died.On(Transit);
        }

        protected override void OnTransitionEndListen()
        {
            enemy.health.died.Off(Transit);
        }
    }
}