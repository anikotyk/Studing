using System.Collections;
using DG.Tweening;
using GameCore.GameScene.Helper.Tasks;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Tasks
{
    public class AnimalEatAutomaticallyTask : HelperTask
    {
        private AnimalProductingView _view;
        private Transform _eatPoint;

        public void Initialize(AnimalProductingView view, Transform eatPoint)
        {
            _view = view;
            _eatPoint = eatPoint;
        }
        
        protected override IEnumerator RunInternal()
        {
            _view.locomotionMovingModule.StartMovement();
            _view.taskModule.MoveTo(_eatPoint.name, _eatPoint.position);
            yield return null;
            float timer = 0;
            while (!_view.taskModule.aiPath.reachedDestination && isRunning && timer < maxTimeMoveToPoint)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            yield return null;
            _view.transform.DORotate(_eatPoint.rotation.eulerAngles, 0.5f).SetLink(_view.gameObject);
            _view.transform.DOMove(_eatPoint.position, 0.5f).SetLink(_view.gameObject);
            yield return new WaitForSeconds(0.5f);
            
            _view.locomotionMovingModule.StopMovement();
            yield return new WaitForSeconds(0.1f);
            _view.animationsModule.PlayEat();
            yield return new WaitForSeconds(0.5f);
            _view.animationsModule.PlayEat();
            yield return new WaitForSeconds(0.5f);
            _view.productionModule.Eat();
        }
    }
}