using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.GameScene_Island.LevelItems.Animal.Tasks;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.Helper.Tasks;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Animal
{
    public class HogLogicModule : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        
        [SerializeField] private BuyObject _objectTurnOnHog;
        [SerializeField] private Transform _appearPoint;
        [SerializeField] private AnimalTrap _trap;
        [SerializeField] private Transform[] _cuttableZones;
        [SerializeField] private int _floor;
        [SerializeField] private int _cntToStole;
        [SerializeField] private float _timeBecomeAlive = 90f;
        [SerializeField] private float _intervalStole = 40f;

        private List<CuttableItem> _cuttableItems => _cuttableZones.SelectMany(obj => obj.GetComponentsInChildren<CuttableItem>()).ToList();
        
        private WalkRandomlyTask _walkRandomlyTask;
        private StoleProductTask _stoleProductTask;
        private GoToTrapTask _goToTrapTask;

        private Tween _resetAnimalTween;
        private Tween _stoleProductTween;
        
        private HogView _viewCached;
        public HogView view => _viewCached ??= GetComponentInParent<HogView>(true);
        private bool _isTurnedOn = false;
        public bool isTurnedOn => _isTurnedOn;

        private bool _isGoToTrap = false;
        public bool isGoToTrap => _isGoToTrap;
        
        private HelperTasksQueueGroup _tasksGroup;

        public override void Construct()
        {
            initializeInOrderController.Add(Initialize, 10000);
        }

        private void Initialize()
        {
            if (!_objectTurnOnHog.isBought)
            {
                view.gameObject.SetActive(false);
                _objectTurnOnHog.onBuy.Once(()=>
                {
                    view.gameObject.SetActive(true);
                    ValidateHog();
                });
            }
            else
            {
                view.gameObject.SetActive(true);
                ValidateHog();
            }
        }
        
        private void ValidateHog()
        {
            AstarPath.active.Scan();
            TurnOnHog();
            SetUpTasks();
        }
        
        public void TurnOnHog()
        {
            _isTurnedOn = true;
            view.taskModule.aiPath.canMove = true;
            if (_tasksGroup != null)
            {
                _tasksGroup.EnableTaskGroup();
                _tasksGroup.RestartTask();
            }
        }

        public void TurnOffHog(bool isToLockMovement = true)
        {
            _isTurnedOn = false;
            if (_tasksGroup != null)
            {
                _tasksGroup.DisableTaskGroup();
            }

            if (isToLockMovement)
            {
                view.taskModule.aiPath.canMove = false;
                view.locomotionMovingModule.StopMoving();
            }
           
        }
        
        private void SetUpTasks()
        {
            SetUpWalkRandomlyTask();
            
            _tasksGroup = new HelperTasksQueueGroup();
            _tasksGroup.Initialize(view, _walkRandomlyTask);

            SetUpStoleProductTask();
            SetUpGoToTrapTask();
        }

        private void SetUpWalkRandomlyTask()
        {
            _walkRandomlyTask = new WalkRandomlyTask();
            _walkRandomlyTask.Initialize(view, _floor);
        }
        
        private void SetUpGoToTrapTask()
        {
            _goToTrapTask = new GoToTrapTask();
            _goToTrapTask.Initialize(view, _trap);

            _trap.onCharged.On(() =>
            {
                _tasksGroup.RunTaskAndCancelOther(_goToTrapTask);
                _isGoToTrap = true;
            });
            
            view.animalInTrapModule.onDead.On(() =>
            {
                OnAnimalDead();
            });

            if (_trap.isCharged)
            {
                _tasksGroup.RunTaskAndCancelOther(_goToTrapTask);
                _isGoToTrap = true;
            }
        }
        
        private void SetUpStoleProductTask()
        {
            _stoleProductTask = new StoleProductTask();
            _stoleProductTask.Initialize(view, _cuttableItems, _cntToStole);
            
            _tasksGroup.RunTask(_stoleProductTask);
        }

        public void ScheduleStole()
        {
            if (_stoleProductTween != null)
            {
                _stoleProductTween.Kill();
            }
            _stoleProductTween = DOVirtual.DelayedCall(_intervalStole, () =>
            {
                _tasksGroup.RunTask(_stoleProductTask);
                ScheduleStole();
            }).SetLink(gameObject);
        }

        public void OnAnimalDead()
        {
            TurnOffHog();
            view.transform.position = _appearPoint.position;
            _isGoToTrap = false;
            view.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack).SetLink(gameObject);

            if (_resetAnimalTween != null)
            {
                _resetAnimalTween.Kill();
            }
            _resetAnimalTween = DOVirtual.DelayedCall(_timeBecomeAlive, () =>
            { 
                OnAnimalAlive();
            }, false).SetLink(gameObject);

            if (_stoleProductTween != null)
            {
                _stoleProductTween.Kill();
            }
        }
        
        public void OnAnimalAlive()
        {
            view.animationsModule.PlayIdle();
            view.transform.position = _appearPoint.position;
            view.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
            {
                TurnOnHog();
            }).SetEase(Ease.OutBack).SetLink(gameObject);
            view.taskModule.MoveTo("CurrentPoint", view.transform.position);
            view.taskModule.aiPath.canMove = true;
            ScheduleStole();
        }
    }
}