using GameCore.Common.LevelItems.Character.CharacterTyping;
using GameCore.Common.Saves;
using JetBrains.Annotations;
using GameBasicsCore.Game.API.Analytics;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.GameScene.UI
{
    public class ChooseCharacterTypeDialog : UIDialogWindow
    {
        [Inject, UsedImplicitly] public CharacterTypeSaveData characterTypeSaveData { get; }
        [InjectOptional, UsedImplicitly] public IDesignAnalytics designAnalytics { get; }
        
        public override bool playSoundOnShowAndHide => false;
        
        [SerializeField] private Button _btnGirl;
        [SerializeField] private Button _btnBoy;
        
        public override void Construct()
        {
            base.Construct();
            
            _btnBoy.onClick.AddListener(() =>
            {
                _btnBoy.interactable = false;
                _btnGirl.interactable = false;
                characterTypeSaveData.value.type = CharacterType.Type.boy;
                
                Debug.Log("Character:Select:Boy");
                designAnalytics?.NewDesignEvent("Character:Select:Boy");
                
                buttonUiClickSfxPlayer?.Play();
                
                Hide();
            });
            _btnGirl.onClick.AddListener(() =>
            {
                _btnBoy.interactable = false;
                _btnGirl.interactable = false;
                characterTypeSaveData.value.type = CharacterType.Type.girl;
                
                Debug.Log("Character:Select:Girl");
                designAnalytics?.NewDesignEvent("Character:Select:Girl");
                
                buttonUiClickSfxPlayer?.Play();
                
                Hide();
            });
        }
    }
}