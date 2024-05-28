using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsSignals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Common.UI
{
    public class GetBubblePopUpView : PopUpView
    {
        [SerializeField] private TextMeshProUGUI _txtTitle;
        [SerializeField] private TextMeshProUGUI _txtDesc;
        [SerializeField] private Button _btn;

        public TheSignal onClick { get; } = new();

        private void Awake()
        {
            _btn.onClick.AddListener(onClick.Dispatch);
        }

        public void SetTitle(string value)
        {
            _txtTitle.text = value;
        }
        
        public void SetDesc(string value)
        {
            _txtDesc.text = value;
        }
    }
}