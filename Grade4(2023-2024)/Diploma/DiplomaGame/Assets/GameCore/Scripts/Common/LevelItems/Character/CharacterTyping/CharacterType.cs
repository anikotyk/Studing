using GameCore.Common.LevelItems.Character.CharacterClothing;
using UnityEngine;

namespace GameCore.Common.LevelItems.Character.CharacterTyping
{
    public class CharacterType : MonoBehaviour
    {
        public enum Type
        {
            boy,
            girl
        }

        [SerializeField] private Type _type;
        public Type type => _type;
        [SerializeField] private GameObject[] _visibleObjects;

        private CharacterDressing _currentCharacterDressing;
        
        public void Activate()
        {
            foreach (var obj in _visibleObjects)
            {
                obj.SetActive(true);
            }
        }
        
        public void Deactivate()
        {
            foreach (var obj in _visibleObjects)
            {
                obj.SetActive(false);
            }
        } 
    }
}