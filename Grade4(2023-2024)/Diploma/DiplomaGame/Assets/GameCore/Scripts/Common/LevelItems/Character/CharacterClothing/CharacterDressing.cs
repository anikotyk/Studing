using System;
using System.Linq;
using GameCore.Common.LevelItems.Character.CharacterTyping;
using UnityEngine;

namespace GameCore.Common.LevelItems.Character.CharacterClothing
{
    public class CharacterDressing : MonoBehaviour
    {
        [SerializeField] private DressingForType[] _dressingForTypes;
        
        public void Activate(CharacterType.Type characterType)
        {
            Deactivate();
            
            foreach (var obj in _dressingForTypes.FirstOrDefault((dressing) => dressing.characterType == characterType).visibleObjects)
            {
                obj.SetActive(true);
            }
        }
        
        public void Deactivate()
        {
            foreach (var dressingForType in _dressingForTypes)
            {
                foreach (var obj in dressingForType.visibleObjects)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
    
    [Serializable]
    public struct DressingForType
    {
        public CharacterType.Type characterType;
        public GameObject[] visibleObjects;
    }
}