using System.Collections.Generic;
using System.Linq;
using GameCore.GameScene.LevelItems.Tutorials;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using Zenject;

namespace GameCore.GameScene.LevelItems.Managers
{
    public class TutorialsManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public TutorialArrow tutorialArrow { get; }
        [Inject, UsedImplicitly] public TutorialArrow3D tutorialArrow3D { get; }

        private List<RaftTutorial> _currentTutorials = new List<RaftTutorial>();
        private RaftTutorial _currentTutorial;

        private List<RaftTutorial> _tutorialsCached;
        public List<RaftTutorial> tutorials
        {
            get
            {
                if (_tutorialsCached == null) _tutorialsCached = GetComponentsInChildren<RaftTutorial>().ToList();
                return _tutorialsCached;
            }
        }

        public override void Construct()
        {
            base.Construct();

            foreach (var tutorial in tutorials)
            {
                tutorial.onStart.On(() =>
                {
                    StartTutorial(tutorial);
                });
                
                tutorial.onComplete.Once(() =>
                {
                    CompleteTutorial(tutorial);
                });
                
                tutorial.onStop.On(() =>
                {
                    StopTutorial(tutorial);
                });
            }
        }

        private void StartTutorial(RaftTutorial tutorial)
        {
            _currentTutorials.Add(tutorial);
            if (_currentTutorials.Count <= 1)
            {
                _currentTutorial = tutorial;
                tutorialArrow.SetTarget(tutorial);
                tutorialArrow3D.SetTarget(tutorial);
            }
        }
        
        private void CompleteTutorial(RaftTutorial tutorial)
        {
            StopTutorial(tutorial);
        }
        
        private void StopTutorial(RaftTutorial tutorial)
        {
            _currentTutorials.Remove(tutorial);
            
            if (_currentTutorial == tutorial)
            {
                tutorialArrow.RemoveTarget();
                tutorialArrow3D.RemoveTarget();
                if (_currentTutorials.Count > 0)
                {
                    StartNextTutorial();
                }
            }
        }

        private void StartNextTutorial()
        {
            _currentTutorial = _currentTutorials[0];
            tutorialArrow.SetTarget(_currentTutorials[0]);
            tutorialArrow3D.SetTarget(_currentTutorials[0]);
        }
    }
}