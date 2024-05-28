using System.Collections;
using DG.Tweening;
using GameCore.GameScene.Helper;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using GameBasicsCore.Game.Misc;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Helper.Tasks
{
    public class HungryRestHelperTask : HelperTask
    {
        private AnimatorParameterApplier _danceAnim;
        private AnimatorParameterApplier _endDanceAnim;
        
        private HelperView _view;

        private SellHelperTask _sellTask;
        
        private Tween _rotationTween;
        private float _rotateTime = 0.35f;
        private ParticleSystem _vfxHungry;

        private bool _isDancing;
        
        private HelperTasksGroup _tasksGroup;
        
        public void Initialize(HelperView view, AnimatorParameterApplier danceAnim,  AnimatorParameterApplier endDanceAnim, SellHelperTask sellTask,
            ParticleSystem vfxHungry)
        {
            _view = view;
            _danceAnim = danceAnim;
            _endDanceAnim = endDanceAnim;
            _sellTask = sellTask;
            _vfxHungry = vfxHungry;
            
            _tasksGroup = new HelperTasksGroup();
            _tasksGroup.Initialize(view);
        }
        
        protected override IEnumerator RunInternal()
        {
            yield return null;
            _tasksGroup.RunTask(_sellTask);
            while (_sellTask.isRunning && isRunning) yield return null;
            
            yield return null;
            _view.taskModule.MoveTo("RestPoint", _view.logicModule.defaultPoint.position); 
            yield return null;
            float timer = 0f;
            while (!_view.taskModule.aiPath.reachedDestination && timer < maxTimeMoveToPoint)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            _view.locomotionMovingModule.StopMovement();
            if (_rotationTween != null)
            {
                _rotationTween.Kill();
            }
            _rotationTween = _view.transform.DORotate(_view.logicModule.defaultPoint.rotation.eulerAngles, _rotateTime).SetLink(_view.gameObject);
            yield return new WaitForSeconds(_rotateTime);
            _view.locomotionMovingModule.StopMovement();
            _vfxHungry.Play();
            _danceAnim.Apply();
            _isDancing = true;
            while (isRunning) yield return null;
            _vfxHungry.Stop();
        }

        protected override void StopInternal()
        {
            if (_isDancing)
            {
                _isDancing = false;
                _endDanceAnim.Apply();
            }

            if (_rotationTween != null)
            {
                _rotationTween.Kill();
            }
            
            _vfxHungry.Stop();
            
            _tasksGroup.StopTask();
        }
    }
}