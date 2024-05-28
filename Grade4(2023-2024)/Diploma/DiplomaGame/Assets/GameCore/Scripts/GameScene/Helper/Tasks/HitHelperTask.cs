using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.Common.LevelItems.Items.HittableItems;
using GameCore.Common.Misc;
using GameCore.GameScene.Helper.Modules;
using UnityEngine;

namespace GameCore.GameScene.Helper.Tasks
{
    public class HitHelperTask : HelperTask
    {
        private List<ProductingHittableItem> _hittables;
        private HelperView _view; 
        private SellHitItemsTask _sellHitItemsTask;
        private HelperTasksGroup _tasksGroup;

        private List<ProductingHittableItem> _usedHittables = new List<ProductingHittableItem>();

        public void Initialize(HelperView view, List<ProductingHittableItem> hittables, SellHitItemsTask sellHitItemsTask)
        {
            _view = view;
            _hittables = hittables;
            _sellHitItemsTask = sellHitItemsTask;
            _tasksGroup = new HelperTasksGroup();
            _tasksGroup.Initialize(view);
        }
        
        protected override IEnumerator RunInternal()
        {
            while (true)
            {
                var hittable = GetHittable();
                if(hittable == null) yield break; 
                yield return null;
                _view.taskModule.MoveTo(hittable.view.name, hittable.helperPosition);
                yield return null;
                float timer = 0;
                while (!_view.taskModule.aiPath.reachedDestination && hittable.isEnabled && timer < maxTimeMoveToPoint)
                {
                    yield return null;
                    timer += Time.deltaTime;
                }
                _view.locomotionMovingModule.StopMovement();
                _view.transform.DOLookAt(hittable.view.transform.position, 0.5f, AxisConstraint.Y, Vector3.up)
                    .SetLink(_view.gameObject);
                yield return new WaitForSeconds(0.5f);
                _view.locomotionMovingModule.StopMovement();
                yield return new WaitForSeconds(1f);
                while (_view.GetModule<HittingCharacterModule>().isNowRunning) yield return null;
                _sellHitItemsTask.SetHitProducts(hittable.spawnedProducts);
                _tasksGroup.RunTask(_sellHitItemsTask);
                while (_sellHitItemsTask.isRunning) yield return null;
            }
        }

        private ProductingHittableItem GetHittable()
        {
            var hittable = _hittables.FirstOrDefault((item) => item.isEnabled && item.view.gameObject.activeInHierarchy 
                                                                              && !_usedHittables.Contains(item) && !item.view.GetComponent<HelperIgnoreItem>());
            if (hittable != null)
            {
                _usedHittables.Add(hittable);
            }
            return hittable;
        }

        protected override void StopInternal()
        {
            base.StopInternal();
            _usedHittables.Clear();
            _tasksGroup.StopTask();
        }
    }
}