using System.Collections;
using DG.Tweening;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.LevelItems.Character.Modules;
using GameBasicsCore.Game.Misc;
using UnityEngine;

namespace GameCore.GameScene.Helper.Tasks
{
    public class RestHelperTask : HelperTask
    {
        private AnimatorParameterApplier _danceAnim;
        private AnimatorParameterApplier _endDanceAnim;
        
        private HelperView _view;

        private SellHelperTask _sellTask;
        
        private Tween _rotationTween;
        private float _rotateTime = 0.35f;

        private bool _isDancing;
        
        private HelperTasksGroup _tasksGroup;
        
        public void Initialize(HelperView view, AnimatorParameterApplier danceAnim,  AnimatorParameterApplier endDanceAnim, SellHelperTask sellTask)
        {
            _view = view;
            _danceAnim = danceAnim;
            _endDanceAnim = endDanceAnim;
            _sellTask = sellTask;
            
            _tasksGroup = new HelperTasksGroup();
            _tasksGroup.Initialize(view);
        }
        
        protected override IEnumerator RunInternal()
        {
            _tasksGroup.RunTask(_sellTask);
            while (_sellTask.isRunning && isRunning) yield return null;
            
            yield return null;
            _view.taskModule.MoveTo("RestPoint", _view.logicModule.defaultPoint.position);
            yield return null;
            float timer = 0;
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
            yield return new WaitForSeconds(0.5f);
            _view.locomotionMovingModule.StopMovement();
            yield return new WaitForSeconds(0.1f);
            _view.GetModule<StackModule>().HideStack();
            _danceAnim.Apply();
            _isDancing = true;
            while (isRunning) yield return null;
        }

        protected override void StopInternal()
        {
            if (_isDancing)
            {
                _isDancing = false;
                _endDanceAnim.Apply();
            }
            _view.GetModule<StackModule>().ShowStack();
            if (_rotationTween != null)
            {
                _rotationTween.Kill();
            }
            
            _tasksGroup.StopTask();
        }
    }
}