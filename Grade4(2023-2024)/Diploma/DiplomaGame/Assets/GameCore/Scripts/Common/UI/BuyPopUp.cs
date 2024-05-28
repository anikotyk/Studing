using System.Collections.Generic;
using DG.Tweening;
using GameBasicsCore.Game.Views.UI.BunchPopUpsMoveAnimations;
using GameBasicsCore.Game.Views.UI.Controls.DataPanels;
using GameBasicsCore.Game.Views.UI.PopUps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Common.UI
{
    public class BuyPopUp : PopUpView
    {
        [SerializeField] private IconLabelDataPanel _priceGO;
        [SerializeField] private Transform _pricesContainer;
        [SerializeField] private TextMeshProUGUI _txtPriceSoftCurrency;
        [SerializeField] private GameObject _softCurrency;
        [SerializeField] private GameObject _iconItemGO;
        [SerializeField] private Transform _scaleTrs;
        [SerializeField] private Image _iconItem;
        [SerializeField] private BunchPopUpsAnimationArea _scArea;

        private Dictionary<string, IconLabelDataPanel> _productPanels = new Dictionary<string, IconLabelDataPanel>();
        public BunchPopUpsAnimationArea scArea => _scArea;

        private Tween _scaleTween;
        
        public void SetPrice(List<PriceProduct> prices, string valueSoftCurrency="")
        {
            if (valueSoftCurrency == "")
            {
                _softCurrency.SetActive(false);
            }
            else
            {
                _softCurrency.SetActive(true);
                _txtPriceSoftCurrency.text = valueSoftCurrency;
            }
            
            foreach (var priceProduct in prices)
            {
                IconLabelDataPanel priceGO;
                if (!_productPanels.ContainsKey(priceProduct.config.id))
                {
                    priceGO = Instantiate(_priceGO, _pricesContainer);
                    priceGO.SetIcon(priceProduct.config.icon);
                    priceGO.gameObject.SetActive(true);
                    _productPanels.Add(priceProduct.config.id, priceGO);
                }
                else
                {
                    priceGO = _productPanels[priceProduct.config.id]; 
                }
               
                priceGO.SetLabel(priceProduct.count +"/"+ priceProduct.price);
                if (priceProduct.count == priceProduct.price)
                {
                    priceGO.gameObject.SetActive(false);
                }
            }
        }

        public void SetItemIcon(Sprite icon)
        {
            _iconItem.sprite = icon;
            if (icon == null)
            {
                _iconItemGO.SetActive(false);
            }
        }

        public void PunchScale()
        {
            if (_scaleTween != null)
            {
                _scaleTween.Kill();
                _scaleTrs.localScale = Vector3.one;
            }
            _scaleTween = _scaleTrs.DOPunchScale(Vector3.one * 0.1f, 0.1f).SetLink(gameObject);
        }
    }
}