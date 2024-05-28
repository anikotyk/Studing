using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Common.UI
{
    public class ButtonLabelDataPanel : MonoBehaviour
    {
        [SerializeField] private Button _btn;
        [SerializeField] private TextMeshProUGUI _txtLabel;

        public Button btn => _btn;
        public TextMeshProUGUI txtLabel => _txtLabel;
        
        public void SetLabel(string value) => _txtLabel.text = value;
        
    }
}