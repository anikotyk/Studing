using GameCore.Common.LevelItems.Character.CharacterTyping;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Character.CharacterClothing
{
    public class CharacterModelClothesManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [SerializeField] private CharacterModelTypeManager _characterModelTypeManager;
        [SerializeField] private CharacterDressing[] _dressings;

        private CharacterDressing _currentDressing;

        public override void Construct()
        {
            base.Construct();
            initializeInOrderController.Add(Initialize, 2000);
        }

        private void Initialize()
        {
            if (_characterModelTypeManager.isToSetAutomatically)
            {
                Validate();
            }
            _characterModelTypeManager.onChangeType.On(Validate);
        }

        private void Validate()
        {
            SetCharacterDressing(_currentDressing);
        }

        public void SetCharacterDressing(CharacterDressing dressing)
        {
            if (dressing == null)
            {
                dressing = _dressings[0];
            }
            _currentDressing = dressing;
            DeactivateAllDressings();
            dressing.Activate(_characterModelTypeManager.currentType);
        }
     
        private void DeactivateAllDressings()
        {
            foreach (var dressing in _dressings)
            {
                dressing.Deactivate();
            }
        }
    }
}