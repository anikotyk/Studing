using GameCore.Common.LevelItems;
using GameCore.GameScene_Island.LevelItems.Animal.Tasks;
using GameCore.GameScene.Helper.Tasks;
using JetBrains.Annotations;
using NaughtyAttributes;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Animal.Modules
{
    public abstract class AnimalProductingLogicModule : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        
        [SerializeField] private bool _useAssignedWalkPoints;
        public bool UseAssignedWalkPoints() => _useAssignedWalkPoints;
        
        [SerializeField, HideIf("UseAssignedWalkPoints")] private int _floor;
        [SerializeField, ShowIf("UseAssignedWalkPoints")] private PointAnimalPath[] _pointsAnimalPath;
        
        private AnimalProductingView _viewCached;
        public AnimalProductingView view => _viewCached ??= GetComponentInParent<AnimalProductingView>(true);
        
        private WalkRandomlyTask _walkRandomlyTask;
        private HelperTasksQueueGroup _tasksGroup;
        protected HelperTasksQueueGroup tasksGroup => _tasksGroup;

        private bool _isApplicationClose;

        public override void Construct()
        {
            initializeInOrderController.Add(Initialize, 50000);
        }

        private void Initialize()
        {
            var buyObject = GetComponentInParent<BuyObject>(true);
            if (buyObject)
            {
                if (buyObject.isBought)
                {
                    ValidateAnimal();
                }
                else
                {
                    buyObject.onBuy.Once(ValidateAnimal);
                }
            }
            else
            {
                ValidateAnimal();
            }
        }

        private void ValidateAnimal()
        {
            AstarPath.active.Scan();
            TurnOnAnimal();
            SetUpTasks();
        }

        private void SetUpTasks()
        {
            SetUpWalkRandomlyTask();
            
            _tasksGroup = new HelperTasksQueueGroup();
            _tasksGroup.Initialize(view, _walkRandomlyTask);

            SetUpEatTask();
            
            _tasksGroup.RunDefaultTask();
        }

        private void SetUpWalkRandomlyTask()
        {
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
        
        protected virtual void SetUpEatTask()
        {
            
        }

        public void TurnOnAnimal()
        {
            view.taskModule.aiPath.canMove = true;
            if (_tasksGroup != null)
            {
                _tasksGroup.RestartTask();
            }
        }
        
        public void TurnOffAnimal()
        {
            if (_tasksGroup != null)
            {
                _tasksGroup.DisableTaskGroup();
            }
            view.taskModule.aiPath.canMove = false;
            view.locomotionMovingModule.StopMovement();
        }

        private void OnApplicationQuit()
        {
            _isApplicationClose = true;
        }

        private void OnDisable()
        {
            if (_isApplicationClose) return;
            if (_tasksGroup != null)
            {
                _tasksGroup.DisableTaskGroup();
            }
        }

        private void OnEnable()
        {
            if (_tasksGroup != null)
            {
                _tasksGroup.EnableTaskGroup();
            }
        }
    }
}