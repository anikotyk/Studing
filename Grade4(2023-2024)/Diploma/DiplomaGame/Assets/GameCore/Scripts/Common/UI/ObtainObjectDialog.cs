using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Common.UI
{
    public class ObtainObjectDialog : UIDialogWindow
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _description;
        
        public void SetObject(Sprite sprite, string objName, string description)
        {
            _image.sprite = sprite;
            _text.text = objName;
            _description.text = description;
            if (description.Trim() == "")
            {
                _description.gameObject.SetActive(false);
            }
        }
    }
}