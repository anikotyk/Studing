using GameCore.GameScene.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;

namespace GameCore.Common.LevelItems.Helper.Modules
{
    public class AutoWateringLogicModule : InteractorCharacterModule
    {
        private WateringCharacterView _viewCached;
        public WateringCharacterView view => _viewCached ??= GetComponentInParent<WateringCharacterView>(true);

        private WaterFilterObject _waterFilterObject;

        private bool _isEnabled;
        
        public void Initialize(WaterFilterObject waterFilterObject)
        {
            _waterFilterObject = waterFilterObject;
            view.transform.position = waterFilterObject.characterMovePoint.position;
            view.transform.rotation = waterFilterObject.characterMovePoint.rotation;
            
            waterFilterObject.onNeedsWater.On(() =>
            {
                if(_isEnabled) waterFilterObject.OnInteracted(view, true);
            });
        }

        public void Enable()
        {
            _isEnabled = true;
            if(_waterFilterObject.isRunningNow)
            {
                character.gameObject.SetActive(false);
                _waterFilterObject.onRunningEnded.Once(() =>
                {
                    character.gameObject.SetActive(true);
                    _waterFilterObject.LockInteractions(view);
                });
            }
            else
            {
                 character.gameObject.SetActive(true);
                 _waterFilterObject.LockInteractions(view);
            }
            
            if(_waterFilterObject.isNeedWater) _waterFilterObject.OnInteracted(view, true);
        }
        
        public void Disable()
        {
            _isEnabled = false;
            _waterFilterObject.UnlockInteractions();
        }
    }
}