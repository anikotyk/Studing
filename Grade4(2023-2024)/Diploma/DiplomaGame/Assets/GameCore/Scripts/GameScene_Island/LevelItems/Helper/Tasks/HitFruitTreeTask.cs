using System.Collections;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene.Helper;
using GameCore.GameScene.Helper.Tasks;
using GameCore.GameScene.LevelItems.Products;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Helper.Tasks
{
    public class HitFruitTreeTask : HelperTask
    {
        private HelperView _view;
        private Vector3 _appleHittingPosition;
        private FruitTreeItem _fruitTreeItem;

        public void Initialize(HelperView view, FruitTreeItem fruitTreeItem)
        {
            _view = view;
            _appleHittingPosition = fruitTreeItem.interactPoint.position;
            _fruitTreeItem = fruitTreeItem;
        }
        
        protected override IEnumerator RunInternal()
        {
            if (!_fruitTreeItem.isEnabled || _view.GetModule<FruitTreeHittingModule>().isRunning) yield break;
            yield return null;
            _view.taskModule.MoveTo("AppleHittingPoint", _appleHittingPosition);
            yield return null;
            float timer = 0f;
            while (!_view.taskModule.aiPath.reachedDestination && _fruitTreeItem.isEnabled && timer < maxTimeMoveToPoint)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            _view.locomotionMovingModule.StopMovement();
            yield return null;
            if (_fruitTreeItem.isEnabled || _view.GetModule<FruitTreeHittingModule>().isRunning)
            {
                _view.taskModule.aiPath.canMove = false;
                _view.locomotionMovingModule.StopMovement();
                yield return new WaitForSeconds(2f);
                _view.locomotionMovingModule.StopMovement();
                while (_view.GetModule<FruitTreeHittingModule>().isRunning) yield return null;
                _view.taskModule.aiPath.canMove = true;
            }
        }

        protected override void StopInternal()
        {
            _view.taskModule.aiPath.canMove = true;
        }
    }
}