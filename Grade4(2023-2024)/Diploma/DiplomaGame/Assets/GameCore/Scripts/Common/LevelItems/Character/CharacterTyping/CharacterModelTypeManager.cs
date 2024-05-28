using System.Linq;
using GameCore.Common.Saves;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Character.CharacterTyping
{
    public class CharacterModelTypeManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public CharacterTypeSaveData characterTypeSaveData { get; }
        
        [SerializeField] private bool _isMainCharacter;
        [SerializeField] private bool _isToSetAutomatically;
        public bool isToSetAutomatically => _isToSetAutomatically;
        [SerializeField] private CharacterType[] _types;

        private CharacterType.Type _currentType;
        public  CharacterType.Type currentType => _currentType;
        
        public TheSignal onChangeType { get; } = new();

        public override void Construct()
        {
            base.Construct();

            if (_isToSetAutomatically)
            {
                characterTypeSaveData.onChange.On((_) =>
                {
                    ValidateCharacterType();
                    onChangeType.Dispatch();
                });
                ValidateCharacterType();
            }
        }

        public void SetCharacterType(CharacterType.Type type)
        {
            _currentType = type;
            CharacterType characterType = GetCharacterType(_currentType);
            DeactivateAllTypes();
            characterType.Activate();
        }
        
        public void ValidateCharacterType()
        {
            if (_isMainCharacter)
            {
                _currentType = characterTypeSaveData.value.type;
            }
            else
            {
                _currentType = characterTypeSaveData.value.type == CharacterType.Type.boy
                    ? CharacterType.Type.girl
                    : CharacterType.Type.boy;
            }

            SetCharacterType(_currentType);
        }

        private void DeactivateAllTypes()
        {
            foreach (var type in _types)
            {
                type.Deactivate();
            }
        }
        
        private CharacterType GetCharacterType(CharacterType.Type type)
        {
            return _types.FirstOrDefault(obj => obj.type == type);
        }
    }
}