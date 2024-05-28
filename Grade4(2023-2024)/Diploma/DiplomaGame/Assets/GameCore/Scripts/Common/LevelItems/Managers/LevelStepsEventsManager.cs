using System.Collections.Generic;
using JetBrains.Annotations;
using GameBasicsCore.Game.API.Analytics;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Managers
{
    public class LevelStepsEventsManager : InjCoreMonoBehaviour
    {
        [SerializeField] private List<LevelEventsManager> _levelEventsManagers;

        public void OnLevelStepState(LevelEventsManager.EventState state, string stepName)
        {
            var manager = GetCurrentLevelEventsManager();
            manager.OnLevelStepState(state, stepName);
        }

        private LevelEventsManager GetCurrentLevelEventsManager()
        {
            if (!_levelEventsManagers[0].isActive || _levelEventsManagers.Count == 1) return _levelEventsManagers[0];
            for (int i = 1; i < _levelEventsManagers.Count; i++)
            {
                if (_levelEventsManagers[i - 1].isActive && !_levelEventsManagers[i].isActive)
                {
                    return _levelEventsManagers[i - 1];
                }
            }
            return _levelEventsManagers[^1];
        }
    }
}