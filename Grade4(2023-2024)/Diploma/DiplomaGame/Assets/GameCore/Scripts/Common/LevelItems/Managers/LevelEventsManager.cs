using JetBrains.Annotations;
using GameBasicsCore.Game.API.Analytics;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Managers
{
    public class LevelEventsManager : InjCoreMonoBehaviour
    {
        [InjectOptional, UsedImplicitly] public IDesignAnalytics designAnalytics { get; }
        
        [SerializeField] private int _levelIndex;
        [SerializeField] private string _levelName;

        public virtual bool isActive => true;
        
        public enum EventState
        {
            Start,
            Complete,
            Fail
        }
        
        private void OnLevelState(EventState state)
        {
            var eventName = "Level:"+state+":" + _levelIndex + "_" + _levelName;
            designAnalytics?.NewDesignEvent(eventName);
            Debug.Log(eventName);
        }
        
        public void OnLevelStepState(EventState state, string stepName)
        {
            var eventName = "LevelStep:"+state+":" + _levelIndex + "_" + _levelName+":"+stepName;
            designAnalytics?.NewDesignEvent(eventName);
            Debug.Log(eventName);
        }

        public void OnLevelStart()
        {
            OnLevelState(EventState.Start);
        }
        
        public void OnLevelComplete()
        {
            OnLevelState(EventState.Complete);
        }
    }
}