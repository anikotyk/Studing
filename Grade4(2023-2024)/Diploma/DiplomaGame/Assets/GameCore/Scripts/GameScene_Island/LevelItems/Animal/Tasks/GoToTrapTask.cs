using System.Collections;
using DG.Tweening;
using GameCore.GameScene_Island.LevelItems.Animal.Modules;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.Helper.Tasks;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Tasks
{
    public class GoToTrapTask : HelperTask
    {
        private HogView _view;
        private AnimalTrap _trap;

        public void Initialize(HogView view,  AnimalTrap trap)
        {
            _view = view;
            _trap = trap;
        }
        
        protected override IEnumerator RunInternal()
        {
            yield return null;
            _view.taskModule.MoveTo("TrapPoint", _trap.animalPoint.position); 
            yield return null;
            float timer = 0;
            while (!_view.taskModule.aiPath.reachedDestination && timer < maxTimeMoveToPoint)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            _view.locomotionMovingModule.StopMovement();
            _view.transform.DORotate(_trap.animalPoint.rotation.eulerAngles, 0.5f);
            yield return new WaitForSeconds(0.5f);
            _view.animationsModule.PlayEat();
            yield return new WaitForSeconds(0.25f);
            _view.GetComponentInChildren<AnimalInTrapModule>().SetInTrap(_trap);
            yield return new WaitForSeconds(0.1f);
            _trap.CloseTrap();
        }
    }
}