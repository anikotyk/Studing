using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using GameBasicsSignals;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.GameScene.UI
{
    public class WateringRunDialog : UIDialogWindow
    {
        [SerializeField] private Image _imgProgress;
        [SerializeField] private Button _cancelBtn;

        public override bool playSoundOnShowAndHide => false;

        public readonly TheSignal onCancel = new();
        
        private void Awake()
        {
            SetProgress(0);
            _cancelBtn.onClick.AddListener(() =>
            {
                _cancelBtn.interactable = false;
                onCancel.Dispatch();
            });
        }

        public void SetProgress(float progress)
        {
            _imgProgress.fillAmount = progress;
        }
    }
}