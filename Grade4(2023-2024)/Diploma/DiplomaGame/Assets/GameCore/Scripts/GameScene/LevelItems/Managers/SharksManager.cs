using DG.Tweening;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.LevelItems.Tutorials;
using GameCore.GameScene.Settings;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Managers
{
    public class SharksManager : MonoBehaviour
    {
        [SerializeField] private MeatAnimalSplinableItem _meatAnimalItem;
        [SerializeField] private SharkTutorial _sharkTutorial;

        private Tween _meatSharkSchedule;

        public float intervalMeatShark => GameplaySettings.def.sharksData.intervalMeatShark;

        private void Start()
        {
            _meatAnimalItem.splineFollower.onEndReached += (_) =>
            {
                _meatAnimalItem.OnReachedEnd();
                OnMeatSharkReachedEnd();
            };
            
            _meatAnimalItem.onDead.On(OnMeatSharkReachedEnd);

            ScheduleMeatShark();
        }

        private void ScheduleMeatShark()
        {
            if (_meatSharkSchedule != null)
            {
                _meatSharkSchedule.Kill();
            }
            
            _meatSharkSchedule = DOVirtual.DelayedCall(intervalMeatShark, () =>
            {
                if (_sharkTutorial.isSharkTutorialPassed.value && _meatAnimalItem.CanStartMove())
                {
                    _meatAnimalItem.Activate();
                }
                else
                {
                    ScheduleMeatShark();
                }
            },false).SetLink(gameObject);
        }

        private void OnMeatSharkReachedEnd()
        {
            ScheduleMeatShark();
        }
    }
}