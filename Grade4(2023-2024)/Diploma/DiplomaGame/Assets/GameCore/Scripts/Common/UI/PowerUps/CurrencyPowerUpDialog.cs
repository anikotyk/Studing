using JetBrains.Annotations;
using GameBasicsCore.Game.Presenters;
using GameBasicsCore.Game.Views.UI.Controls;
using GameBasicsCore.Game.Views.UI.Controls.DataPanels;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using GameBasicsSignals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.Common.UI.PowerUps
{
    public class CurrencyPowerUpDialog : UIDialogWindow
    {
        public enum CurrencyType
        {
            SoftCurrency
        };
        
        [Inject, UsedImplicitly] public RvTktUIControlsPresenter rvTktUIControlsPresenter { get; }
        [SerializeField] private TextMeshProUGUI _txtTitle;
        [SerializeField] private IconLabelDataPanel _scPanel;
        [SerializeField] private RvTktButtonsPanel _rvTktButtonsPanel;
        [SerializeField] private Button _btn;

        public readonly TheSignal onClaim = new();
        public readonly TheSignal onSkip = new();
        
        protected override void Init()
        {
            base.Init();

            _rvTktButtonsPanel.Init(rvTktUIControlsPresenter);

            _rvTktButtonsPanel.onSuccess.On(OnClaim);
            _rvTktButtonsPanel.onClick.On(() =>
            {
                buttonUiClickSfxPlayer?.Play();
            });
            _btn.onClick.AddListener(()=>
            {
                buttonUiClickSfxPlayer?.Play();
                OnClaim();
            });
            
            btnClose.onClick.AddListener(OnSkip);
        }

        public void SetRvTktPlacementName(string rvTktPlacementName)
        {
            _rvTktButtonsPanel.SetPlacementName(rvTktPlacementName);
        }
        
        public void SetAmountCurrency(int amount, CurrencyType currencyType)
        {
            var panel = SetCurrencyType(currencyType);
            panel.SetLabel(amount+"");
        }
        
        public void SetTitle(string text)
        {
            _txtTitle.text = text;
        }


        private IconLabelDataPanel SetCurrencyType(CurrencyType currencyType)
        {
            _scPanel.gameObject.SetActive(false);
            
            if (currencyType == CurrencyType.SoftCurrency)
            {
                _scPanel.gameObject.SetActive(true);
                return _scPanel;
            }

            return null;
        }

        public void SetIsFree(bool isFree)
        {
            _btn.gameObject.SetActive(isFree);
            _rvTktButtonsPanel.gameObject.SetActive(!isFree);
        }
        
        private void OnSkip()
        {
            onSkip.Dispatch();
            btnClose.onClick.RemoveAllListeners();
            
            Hide();
        }

        private void OnClaim()
        {
            _rvTktButtonsPanel.SetInteractable(false);
            onClaim.Dispatch();
            
            Hide();
        }
    }
}
