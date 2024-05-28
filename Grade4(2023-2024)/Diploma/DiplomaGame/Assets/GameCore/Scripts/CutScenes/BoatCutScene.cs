using System.Collections.Generic;
using GameCore.ShipScene.Extentions;
using GameBasicsCore.Game.Misc;
using UnityEngine;

namespace GameCore.CutScenes
{
    public class BoatCutScene : MonoBehaviour
    {
        [SerializeField] private List<BoatCutSceneCharacter> _characters;
        [SerializeField] private List<ReversibleAnimatorApplier> _animations;
        [SerializeField] private AnimatorParameterApplier _idleAnimation;

        public List<BoatCutSceneCharacter> characters => _characters;

        private void OnDisable()
        {
            foreach (var character in characters)
            {
                foreach (var applier in _animations)
                {
                    applier.SetAnimator(character.animator);
                    applier.Revert();
                }
                _idleAnimation.SetAnimator(character.animator);
                _idleAnimation.Apply();
            }
        }

        public void MoveCharactersToDefault()
        {
            foreach (var character in _characters)
            {
                var characterTransform = character.model.transform;
                characterTransform.position = character.startPoint.position;
                characterTransform.rotation = character.startPoint.rotation;
            }
        }
    }
}