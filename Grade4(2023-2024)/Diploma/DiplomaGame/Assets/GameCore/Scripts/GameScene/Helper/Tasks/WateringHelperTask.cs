using System.Collections;
using GameCore.GameScene.LevelItems.Character.Modules;
using GameCore.GameScene.LevelItems.Products;
using UnityEngine;

namespace GameCore.GameScene.Helper.Tasks
{
    public class WateringHelperTask : HelperTask
    {
        private HelperView _view;
        private Vector3 _wateringPosition;
        private WaterFilterObject _waterFilterObject;

        public void Initialize(HelperView view, WaterFilterObject waterFilter)
        {
            _view = view;
            _wateringPosition = waterFilter.transform.position;
            _waterFilterObject = waterFilter;
        }
        
        protected override IEnumerator RunInternal()
        {
            yield return null;
            if (!_waterFilterObject.isEnabled || !_waterFilterObject.isNeedWater || _view.GetModule<WateringModule>().isRunning) yield break;
            yield return null;
            _view.taskModule.MoveTo("WateringPoint", _wateringPosition);
            yield return null;
            float timer = 0;
            while (!_view.taskModule.aiPath.reachedDestination && _waterFilterObject.isEnabled &&
                   _waterFilterObject.isNeedWater && timer < maxTimeMoveToPoint)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            _view.locomotionMovingModule.StopMovement();
            yield return null;
            if (_waterFilterObject.isEnabled && _waterFilterObject.isNeedWater || _view.GetModule<WateringModule>().isRunning)
            {
                yield return new WaitForSeconds(0.75f);
                if (!_view.GetModule<WateringModule>().isRunning && _waterFilterObject.isEnabled && _waterFilterObject.isNeedWater)
                {
                    _view.locomotionMovingModule.StopMovement();
                    yield return new WaitForSeconds(0.75f);
                }
                while (_view.GetModule<WateringModule>().isRunning) yield return null;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}