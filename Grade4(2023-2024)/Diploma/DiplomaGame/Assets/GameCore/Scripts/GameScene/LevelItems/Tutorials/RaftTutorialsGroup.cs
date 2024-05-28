using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Tutorials
{
    public abstract class RaftTutorialsGroup : InjCoreMonoBehaviour
    {
        private List<RaftTutorial> _tutorialsCached;
        public List<RaftTutorial> tutorials
        {
            get
            {
                if (_tutorialsCached == null) _tutorialsCached = GetComponentsInChildren<RaftTutorial>().ToList();
                return _tutorialsCached;
            }
        }

        private RaftTutorial _currentTutorial;
        private int index = 0;
        
        public virtual void StartTutorialsGroup()
        {
            foreach (var tutorial in tutorials)
            {
                tutorial.onComplete.Once(() =>
                {
                    if (_currentTutorial == tutorial)
                    {
                        DOVirtual.DelayedCall(0.01f, StartNextTutorial,false).SetLink(gameObject);
                    }
                });
            }

            StartNextTutorial();
        }

        private void StartNextTutorial()
        {
            if (index >= tutorials.Count) return;
            
            _currentTutorial = tutorials[index];
            index++;
            
            if (!_currentTutorial.isCompleted)
            {
                _currentTutorial.StartTutorial();
            }
            else
            {
                StartNextTutorial();
            }
        }
    }
}