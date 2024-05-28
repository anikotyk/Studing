using GameCore.Common.LevelItems.Character.CharacterClothing;
using GameCore.Common.LevelItems.Character.CharacterTyping;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using UnityEngine;

namespace GameCore.Common.LevelItems.Character.Modules
{
    public class SpeedSuperPowerModule : InteractorCharacterModule
    {
        [SerializeField] private CharacterDressing _dressing;
        [SerializeField] private CharacterModelTypeManager _characterModelTypeManager;
        
        public void OnSuperPower()
        {
            _dressing.Activate(_characterModelTypeManager.currentType);
        }
        
        public void OnEndSuperPower()
        {
            _dressing.Deactivate();
        }
    }
}