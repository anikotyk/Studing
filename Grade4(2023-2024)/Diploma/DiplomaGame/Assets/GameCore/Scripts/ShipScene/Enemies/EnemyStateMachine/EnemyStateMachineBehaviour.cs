using GameBasicsCore.Game.Core;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine
{
    public class EnemyStateMachineBehaviour : InjCoreMonoBehaviour
    {
        private EnemyStateMachine _stateMachineCached;
        public EnemyStateMachine stateMachine
        {
            get
            {
                if (_stateMachineCached == null)
                    _stateMachineCached = GetComponentInParent<EnemyStateMachine>(true);
                return _stateMachineCached;
            }
        }
        
        public Enemy enemy => stateMachine.enemy;
    }
}