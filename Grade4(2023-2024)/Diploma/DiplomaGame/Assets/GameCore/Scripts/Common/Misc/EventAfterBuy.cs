using GameCore.Common.LevelItems.Managers;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Misc
{
    public class EventAfterBuy : ActionAfterBuy
    {
        [InjectOptional, UsedImplicitly] public LevelStepsEventsManager levelStepsEventsManager { get; }
        
        [SerializeField] private string _eventName;

        protected override void Action()
        {
            base.Action();
            levelStepsEventsManager.OnLevelStepState(LevelEventsManager.EventState.Complete, _eventName);
        }
    }
}