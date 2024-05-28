using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using GameBasicsSignals;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Common.UI
{
    public class CancelDialog : UIDialogWindow
    {
        [SerializeField] private Button _cancelBtn;
        
        public override bool playSoundOnShowAndHide => false;
        
        public readonly TheSignal onCancel = new();
        
        private void Awake()
        {
            _cancelBtn.onClick.AddListener(() =>
            {
                _cancelBtn.interactable = false;
                onCancel.Dispatch();
                buttonUiClickSfxPlayer?.Play();
            });
        }
    }
}