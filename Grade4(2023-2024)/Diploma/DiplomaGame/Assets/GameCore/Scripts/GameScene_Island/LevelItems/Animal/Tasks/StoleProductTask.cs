using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.Helper.Tasks;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Tasks
{
    public class StoleProductTask : HelperTask
    {
        private HogView _view;
        private List<CuttableItem> _cuttableItems;
        private int _cntToStole;
        
        private List<CuttableItem> _usedItems = new List<CuttableItem>();

        public void Initialize(HogView view, List<CuttableItem> cuttableItems, int cntToStole)
        {
            _view = view;
            _cuttableItems = cuttableItems;
            _cntToStole = cntToStole;
        }
        
        protected override IEnumerator RunInternal()
        {
            yield return null;

            int cnt = 0;
            while (cnt < _cntToStole)
            {
                var prod = GetProduct();
                if (!prod) break;
                _usedItems.Add(prod);
                _view.taskModule.MoveTo("StoleProductPoint", prod.transform.position + Vector3.forward * 0.75f); 
                yield return null;
                float timer = 0;
                while (!_view.taskModule.aiPath.reachedDestination && prod.interactItem.enabled && timer < maxTimeMoveToPoint)
                {
                    yield return null;
                    timer += Time.deltaTime;
                }
                if(!prod.interactItem.enabled) continue;
                _view.locomotionMovingModule.StopMovement();
                _view.transform.DOLookAt(prod.transform.position, 0.5f).SetEase(Ease.Linear).SetLink(_view.gameObject);
                yield return new WaitForSeconds(0.5f);
                if(!prod.interactItem.enabled) continue;
                _view.animationsModule.PlayEat();
                yield return new WaitForSeconds(0.5f);
                if(!prod.interactItem.enabled) continue;
                prod.Cut(false, false);
                cnt++;
            }
            
            _view.hogLogicModule.ScheduleStole();
        }

        protected override void StopInternal()
        {
            base.StopInternal();
            _usedItems.Clear();
        }

        private CuttableItem GetProduct()
        {
            return _cuttableItems.FirstOrDefault((item) =>
                item.gameObject.activeInHierarchy && item.interactItem.enabled && !_usedItems.Contains(item));
        }
    }
}