
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using UnityEngine;

namespace GameCore.GameScene_Iceland.LevelItems.Helper.Modules
{
    public class HelperColdModule : InteractorCharacterModule
    {
        [SerializeField] private Transform _coldPosition;
        [SerializeField] private AnimatorParameterApplier _coldAnim;
        [SerializeField] private AnimatorParameterApplier _endColdAnim;

        private bool _isToApplyRigidbody;
        private bool _useGravity;
        private bool _isKinematic;

        public void SetCold()
        {
            character.transform.position = _coldPosition.position;
            _coldAnim.Apply();
            
            _useGravity = character.rigidbody.useGravity;
            _isKinematic = character.rigidbody.isKinematic;
            character.rigidbody.useGravity = false;
            character.rigidbody.isKinematic = true;

            _isToApplyRigidbody = true;
        }
        
        public void SetNotCold()
        {
            _endColdAnim.Apply();

            if (_isToApplyRigidbody)
            {
                character.rigidbody.useGravity = _useGravity;
                character.rigidbody.isKinematic = _isKinematic;
            }
        }
    }
}