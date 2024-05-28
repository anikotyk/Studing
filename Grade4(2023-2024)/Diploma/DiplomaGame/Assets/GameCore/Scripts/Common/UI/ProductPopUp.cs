using System.Collections.Generic;
using DG.Tweening;
using MPUIKIT;
using GameBasicsCore.Game.Views.UI.Controls.DataPanels;
using GameBasicsCore.Game.Views.UI.PopUps;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Common.UI
{
    public class ProductPopUp : PopUpView
    {
        [SerializeField] private IconLabelDataPanel _priceGO;
        [SerializeField] private Transform _pricesContainer;
        [SerializeField] private GameObject _iconItemGO;
        [SerializeField] private Transform _scaleTrs;
        [SerializeField] private Image _iconItem; 
        [SerializeField] private MPImageBasic _progess;
        [SerializeField] private Color _colorNotEnough;
        [SerializeField] private Color _colorEnough;

        private Dictionary<string, IconLabelDataPanel> _productPanels = new Dictionary<string, IconLabelDataPanel>();

        private Tween _scaleTween;
        
        public void SetPrice(List<PriceProductCarrier> prices)
        {
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
                if (priceProduct.count < priceProduct.price)
                {
                    priceGO.txtLabel.color = _colorNotEnough;
                }
                else
                {
                    priceGO.txtLabel.color = _colorEnough;
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
        
        public void SetProgress(float progess)
        {
            _progess.fillAmount = progess;
        }
    }
}