using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.Helper;
using GameCore.GameScene.Helper.Tasks;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Helper.Tasks
{
    public class CutItemsTask : HelperTask
    {
        private HelperView _view;
        private CuttableItem[] _cuttableItemsCached;
        public CuttableItem[] cuttableItems
        {
            get
            {
                if (_cuttableItemsCached == null) _cuttableItemsCached = GameObject.FindObjectsOfType<CuttableItem>(true).ToList().FindAll(item => cuttingModule.IsAbleToCut(item.spawnProductConfig)).ToArray();
                return _cuttableItemsCached;
            }
        }

        private Tween _rotateTween;

        public CuttingModule cuttingModule => _view.GetModule<CuttingModule>();

        private List<CuttableItem> _usedItems = new List<CuttableItem>();

        public void Initialize(HelperView view)
        {
            _view = view;
        }

        public bool CanRun()
        {
            return GetItemToCut()!=null;
        }
        
        protected override IEnumerator RunInternal()
        {
            while (true)
            {
                var item = GetItemToCut();
                if(item == null) break;
                _usedItems.Add(item);
                yield return null;
                Vector3 pos = item.transform.position;
                pos.y = _view.transform.position.y;
                pos.x -= 0.5f;
                _view.taskModule.MoveTo(item.name, pos);
                yield return null;
                float timer = 0f;
                while (!_view.taskModule.aiPath.reachedDestination && item.interactItem.enabled && timer < maxTimeMoveToPoint)
                {
                    yield return null;
                    timer += Time.deltaTime;
                }
                yield return null;
               
                if (item.interactItem.enabled)
                { 
                    _view.locomotionMovingModule.StopMovement();
                    if (_rotateTween != null)
                    {
                        _rotateTween.Kill();
                    }
                    _rotateTween = _view.transform.DOLookAt(item.transform.position, 0.25f).SetLink(_view.gameObject);
                    yield return new WaitForSeconds(0.25f);
                    _view.locomotionMovingModule.StopMovement();
                    yield return new WaitForSeconds(0.5f);
                    if (item.interactItem.enabled && !cuttingModule.isRunning)
                    {
                        cuttingModule.StartCutting();
                    }
                    yield return new WaitForSeconds(1f);
                }
            }
        }

        protected override void StopInternal()
        {
            base.StopInternal();
            _usedItems.Clear();
        }

        private CuttableItem GetItemToCut()
        {
            return cuttableItems.FirstOrDefault(item => item.interactItem.enabled && item.gameObject.activeInHierarchy && !_usedItems.Contains(item));
        }
    }
}