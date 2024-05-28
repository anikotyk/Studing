using GameCore.GameScene_Island.LevelItems.Animal;
using GameCore.GameScene_Island.LevelItems.Animal.Tasks;
using GameCore.GameScene.Helper.Modules;
using NaughtyAttributes;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.Common.LevelItems.Animals.FightAnimal.Modules
{
    public class AnimalWalkRandomModule : InjCoreMonoBehaviour
    {
        [SerializeField] private bool _useAssignedWalkPoints;
        public bool UseAssignedWalkPoints() => _useAssignedWalkPoints;
        
        [SerializeField, HideIf("UseAssignedWalkPoints")] private int _floor;
        [SerializeField, ShowIf("UseAssignedWalkPoints")] private PointAnimalPath[] _pointsAnimalPath;
        
        private FightAnimalView _viewCached;
        public FightAnimalView view => _viewCached ??= GetComponentInParent<FightAnimalView>(true);

        private HelperTasksGroup _tasksGroup;
        private WalkRandomlyTask _walkRandomlyTask;

        public override void Construct()
        {
            base.Construct();
            _tasksGroup = new HelperTasksGroup();
            _tasksGroup.Initialize(view);
            _walkRandomlyTask = new WalkRandomlyTask();
            if (UseAssignedWalkPoints())
            {
                _walkRandomlyTask.Initialize(view, _pointsAnimalPath);
            }
            else
            {
                _walkRandomlyTask.Initialize(view, _floor);
            }
        }

        public void StartWalkRandomly()
        {
            _tasksGroup.RunTask(_walkRandomlyTask);
        }

        public void StopWalkRandomly()
        {
            _tasksGroup.StopTask();
        }
    }
}