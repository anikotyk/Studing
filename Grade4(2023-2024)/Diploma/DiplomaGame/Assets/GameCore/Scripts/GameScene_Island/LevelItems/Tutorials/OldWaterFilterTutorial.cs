using GameCore.Common.Misc;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.LevelItems.Tutorials;
using GameBasicsCore.Game.SaveProperties.Api;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Tutorials
{
    public class OldWaterFilterTutorial : RaftTutorial
    {
        [SerializeField] private OldFilter _oldFilter;
        
        public override Vector3 targetPos => _oldFilter.transform.position + Vector3.up * 0.5f;

        private TheSaveProperty<bool> _isOldFilterTutorialPassed;
        
        public override void Construct()
        {
            base.Construct();
            
            _isOldFilterTutorialPassed = new(CommStr.OldFilterTutorialPassed);
           
            if (!_isOldFilterTutorialPassed.value)
            {
                _oldFilter.onReadyToTake.On(StartTutorial);
                _oldFilter.onTaken.Once(CompleteTutorial);
            }
        }

        public override void CompleteTutorial()
        {
            base.CompleteTutorial();

            _isOldFilterTutorialPassed.value = true;
        }
    }
}