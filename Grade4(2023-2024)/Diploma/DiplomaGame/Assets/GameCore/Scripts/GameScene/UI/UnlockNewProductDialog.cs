using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.GameScene.UI
{
    public class UnlockNewProductDialog : UIDialogWindow
    {
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _iconOutline;

        public void Initialize(string title, int price, Sprite icon, Sprite iconOutline)
        {
            _txtCaption.text = title;
            _priceText.text = price+"";
            _icon.sprite = icon;
            _iconOutline.sprite = iconOutline;
        }
    }
}