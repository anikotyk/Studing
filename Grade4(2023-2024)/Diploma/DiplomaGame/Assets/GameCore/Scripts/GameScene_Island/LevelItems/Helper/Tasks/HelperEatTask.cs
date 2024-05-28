using System.Collections;
using DG.Tweening;
using GameCore.GameScene_Island.LevelItems.Platforms;
using GameCore.GameScene.Helper;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using GameBasicsCore.Game.Misc;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Helper.Tasks
{
    public class HelperEatTask : HelperTask
    {
        private HelperView _view;
        private Transform _targetPoint;
        private SellProductsCollectPlatform _sellProductsCollectPlatform;
        private AnimatorParameterApplier _eatAnim;
        
        private SellHelperTask _sellTask;
        private HelperTasksGroup _tasksGroup;
        
        public readonly TheSignal onEat = new();

        public void Initialize(HelperView view, Transform targetPoint,  AnimatorParameterApplier eatAnim, 
            SellProductsCollectPlatform sellProductsCollectPlatform, SellHelperTask sellTask)
        {
            _view = view;
            _targetPoint = targetPoint;
            _sellProductsCollectPlatform = sellProductsCollectPlatform;
            _eatAnim = eatAnim;
            _sellTask = sellTask;
            
            _tasksGroup = new HelperTasksGroup();
            _tasksGroup.Initialize(view);
        }

        protected override IEnumerator RunInternal()
        {
            yield return null;
            _tasksGroup.RunTask(_sellTask);
            while (_sellTask.isRunning && isRunning) yield return null;
            yield return null;
            if(_sellProductsCollectPlatform.productsCarrier.count <= 0) yield break;
            yield return null;
            _view.taskModule.MoveTo(_sellProductsCollectPlatform.name, _targetPoint.position);
            yield return null;
            float timer = 0f;
            while (!_view.taskModule.aiPath.reachedDestination && timer < maxTimeMoveToPoint)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            _view.locomotionMovingModule.StopMovement();
            _view.transform.DORotate(_targetPoint.rotation.eulerAngles, 0.5f).SetLink(_view.gameObject);
            yield return new WaitForSeconds(0.5f);
            if (_sellProductsCollectPlatform.productsCarrier.count > 0)
            {
                _eatAnim.Apply();
                var prod = _sellProductsCollectPlatform.productsCarrier.GetOutLast();
                prod.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).SetLink(_view.gameObject);
                yield return new WaitForSeconds(0.5f);
                Object.Destroy(prod.gameObject);
                yield return new WaitForSeconds(1.5f);
                onEat.Dispatch();
            }
        }
        
        protected override void StopInternal()
        {
            base.StopInternal();
            _tasksGroup.StopTask();
        }
    }
}