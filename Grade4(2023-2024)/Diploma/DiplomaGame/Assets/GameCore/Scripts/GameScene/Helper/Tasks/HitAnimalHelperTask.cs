using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.LevelItems.Animals.FightAnimal;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.GameScene.Helper.Modules;
using UnityEngine;

namespace GameCore.GameScene.Helper.Tasks
{
    public class HitAnimalHelperTask : HelperTask
    {
        private List<FightAnimalView> _animals;
        private HelperView _view; 
        private SellHitItemsTask _sellHitItemsTask;
        private HelperTasksGroup _tasksGroup;
        private int _helperIndex;

        public void Initialize(HelperView view, List<FightAnimalView> animals, int helperIndex, SellHitItemsTask sellHitItemsTask)
        {
            _view = view;
            _animals = animals;
            _helperIndex = helperIndex;
            _sellHitItemsTask = sellHitItemsTask;
            _tasksGroup = new HelperTasksGroup();
            _tasksGroup.Initialize(view);
        }
        
        protected override IEnumerator RunInternal()
        {
            while (true)
            {
                var animal = GetHittable();
                if(animal == null) yield break; 
                yield return null;
                
                while (animal.isActive && isRunning)
                {
                    int index = _helperIndex % animal.helperPoints.Length;
                    var point = animal.helperPoints[index].transform;
                    _view.taskModule.Follow(animal.transform.name, point);
                    yield return null;
                    while (!_view.taskModule.aiPath.reachedDestination && animal.isActive)
                    {
                        yield return null;
                    }
                    _view.locomotionMovingModule.StopMovement();
                    _view.taskModule.StopFollow();
                    _view.transform.DOLookAt(animal.transform.position, 0.5f).SetLink(_view.gameObject);
                    yield return new WaitForSeconds(1f);
                    while (_view.GetModule<HittingCharacterModule>().isNowRunning) yield return null;
                }
                if (!animal.isActive)
                {
                    yield return new WaitForSeconds(1f);
                    float timer = 0f;
                    while (animal.fightAnimalGetDamageModule.spawnedProducts.Count <= 0 && timer < 5f)
                    {
                        yield return null;
                        timer += Time.deltaTime;
                    }
                    yield return new WaitForSeconds(0.1f);
                    if (animal.fightAnimalGetDamageModule.spawnedProducts.Count > 0)
                    {
                        _sellHitItemsTask.SetHitProducts(animal.fightAnimalGetDamageModule.spawnedProducts);
                        _tasksGroup.RunTask(_sellHitItemsTask);
                        yield return null;
                        while (_sellHitItemsTask.isRunning) yield return null;
                    }
                }
            }
        }

        private FightAnimalView GetHittable()
        {
            var animal = _animals.FirstOrDefault((item) => item.isActive);
            return animal;
        }

        protected override void StopInternal()
        {
            base.StopInternal();
            _tasksGroup.StopTask();
        }
    }
}