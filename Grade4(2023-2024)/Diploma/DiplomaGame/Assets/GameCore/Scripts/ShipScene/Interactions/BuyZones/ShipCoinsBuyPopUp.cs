using GameBasicsCore.Game.Views.UI.BunchPopUpsMoveAnimations;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsSDK.GameBasicsCore.Plugins.Tools.GameBasicsTools.LittleHelpers;
using TMPro;
using UnityEngine;

namespace GameCore.ShipScene.Interactions
{
    public class ShipCoinsBuyPopUp : PopUpView
    {
        [SerializeField] private TMP_Text _priceText;
        
        [SerializeField] private GameObject _labelModel;
        [SerializeField] private TMP_Text _labelText;
        [SerializeField] private CooldownPulseAnimation _pulseAnimation;
        [SerializeField] private BunchPopUpsAnimationArea _scArea;
        public BunchPopUpsAnimationArea scArea => _scArea;

        public void ShowLabel(string label)
        {
            _labelText.text = label;
            _labelModel.SetActive(true);
        }

        public void HideLabel()
        {
            _labelModel.SetActive(false);
        }
        
        public void SetPrice(int price)
        {
            _priceText.text = price.ToString();
        }

        public void Punch()
        {
            _pulseAnimation.PlaySimple();
        }
    }
}