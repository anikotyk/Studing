using GameBasicsSignals;
using UnityEngine;

namespace GameCore.ShipScene.Enemies.Combat
{
    public class EnemyAttackModule : MonoBehaviour
    {
        public TheSignal started { get; } = new();
        public TheSignal attacked { get; } = new();
        public TheSignal stopped { get; } = new();

        public EnemyAttackStates currentState { get; private set; } = EnemyAttackStates.Ended;

        public void TriggerStarted()
        {
            currentState = EnemyAttackStates.Started;
            started.Dispatch();
        } 
        public void TriggerAttacked()
        {
            currentState = EnemyAttackStates.Started;
            attacked.Dispatch();
        } 
        public void TriggerStopped()
        {
            currentState = EnemyAttackStates.Ended;
            stopped.Dispatch();
        } 
        
    }

    public enum EnemyAttackStates
    {
        Started, 
        Ended
    }
}