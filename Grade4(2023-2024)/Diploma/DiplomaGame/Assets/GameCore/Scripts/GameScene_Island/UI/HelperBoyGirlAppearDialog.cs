using GameCore.Common.LevelItems.Character.CharacterTyping;
using GameCore.Common.Saves;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.GameScene_Island.UI
{
    public class HelperBoyGirlAppearDialog : HelperAppearDialog
    {
        [Inject, UsedImplicitly] public CharacterTypeSaveData characterTypeSaveData { get; }
        
        [SerializeField] private Image _characterImage;
        [SerializeField] private Sprite _girlSprite;
        [SerializeField] private Sprite _boySprite;

        public override void Construct()
        {
            base.Construct();
            
            if (characterTypeSaveData.value.type == CharacterType.Type.boy)
            {
                _characterImage.sprite = _girlSprite;
            }
            else
            {
                _characterImage.sprite = _boySprite;
            }
        }
    }
}