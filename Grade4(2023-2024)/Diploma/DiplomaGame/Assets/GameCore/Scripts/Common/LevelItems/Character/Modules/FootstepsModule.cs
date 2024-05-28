using GameCore.Common.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using UnityEngine;

namespace GameCore.Common.LevelItems.Character.Modules
{
    public class FootstepsModule : InteractorCharacterModule
    {
        [SerializeField] private FootprintsEffect _footprintsEffect;
        [SerializeField] private FootstepsHandler _footstepsHandler;
        
        [SerializeField] private AudioSource _footstepSound;
        
        public override void Construct()
        {
            base.Construct();
            _footstepsHandler.onLeftStep.On(LeftStep);
            _footstepsHandler.onRightStep.On(RightStep);
        }

        public void LeftStep()
        {
            if(_footprintsEffect!=null) _footprintsEffect.ShowLeftFootprint();
            if(_footstepSound!=null) _footstepSound.Play();
        }
    
        public void RightStep()
        {
            if(_footprintsEffect!=null) _footprintsEffect.ShowRightFootprint();
            if(_footstepSound!=null) _footstepSound.Play();
        }
    }
}