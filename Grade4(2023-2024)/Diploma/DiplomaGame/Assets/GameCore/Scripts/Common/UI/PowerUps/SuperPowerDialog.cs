using DG.Tweening;
using GameCore.Common.Misc;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using GameBasicsCore.Tools.Extensions;
using GameBasicsSignals;
using TMPro;
using UnityEngine;

namespace GameCore.Common.UI.PowerUps
{
    public class SuperPowerDialog : UIDialogWindow
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Transform _rays;
        [SerializeField] private Transform _modelContainer;
       
        [SerializeField] private PowerUp3D _powerUp3D;

        private PowerUpClaimModePanel _claimModePanelCached;
        public PowerUpClaimModePanel claimModePanel => _claimModePanelCached ??= GetComponentInChildren<PowerUpClaimModePanel>(true);
        
        private PowerUpClaimModeWithAdsPanel _claimModeWithAdsPanelCached;
        public PowerUpClaimModeWithAdsPanel claimModeWithAdsPanel => _claimModeWithAdsPanelCached ??= GetComponentInChildren<PowerUpClaimModeWithAdsPanel>(true);

        private Transform _panelsParent;
        
        public readonly TheSignal onClaimSuperPower = new();
        public readonly TheSignal onSkipSuperPower = new();

        private bool _isFreeClaim = false;
        
        protected override void Init()
        {
            base.Init();

            claimModePanel.onClaimClick.On(()=>
            {
                OnClaim();
                buttonUiClickSfxPlayer?.Play();
            });
            claimModeWithAdsPanel.onRvTktSuccess.On(OnClaim);
            claimModeWithAdsPanel.onRvTktClick.On(()=>
            {
                buttonUiClickSfxPlayer?.Play();
            });
            btnClose.onClick.AddListener(OnSkip);
            
            _powerUp3D.gameObject.SetLayerRecursively(NCStr.UI);
        }

        public void SetData(string title, string description, GameObject modelPrefab, string rvName = CommStr.Rv_ClaimSuperPower)
        {
            _titleText.text = title;
            _descriptionText.text = description;
            var model = Instantiate(modelPrefab, _modelContainer);
            model.transform.localPosition = Vector3.zero;
            model.transform.localScale = Vector3.one;
            model.transform.localRotation = Quaternion.Euler(Vector3.zero);;

            claimModeWithAdsPanel.SetPlacementRvName(rvName);
        }
        
        public void SetIsFreeClaim(bool isFreeClaim)
        {
            _isFreeClaim = isFreeClaim;
            ValidatePanel();
        }

        private void OnSkip()
        {
            onSkipSuperPower.Dispatch();
            btnClose.onClick.RemoveAllListeners();
            
            Hide();
        }
        
        private async void OnClaim()
        {
            onClaimSuperPower.Dispatch();

            await _powerUp3D.transform.DOPunchScale(Vector3.one * 1.5f, 0.4f, 1).AsyncWaitForCompletion();
            
            Hide();
        }

        private async void ValidatePanel()
        {
            if (!_isFreeClaim)
            {
                claimModeWithAdsPanel.gameObject.SetActive(true);
                _panelsParent =  claimModeWithAdsPanel.transform.parent;
                
                btnClose.gameObject.SetActive(true);
                claimModePanel.gameObject.SetActive(false);
            }
            else
            {
                claimModePanel.gameObject.SetActive(true);
                _panelsParent =  claimModePanel.transform.parent;
                
                btnClose.gameObject.SetActive(false);
                claimModeWithAdsPanel.gameObject.SetActive(false);
            }
            
            _rays.gameObject.SetActive(true);
            _rays.transform.localScale = Vector3.zero;
            _rays.DOScale(1f, 0.5f);
            
            await _panelsParent.DOScale(1f, 0.5f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }

        protected override void ShowContent(Sequence seq)
        {
            _powerUp3D.Appear(seq);
        }
    }
}
