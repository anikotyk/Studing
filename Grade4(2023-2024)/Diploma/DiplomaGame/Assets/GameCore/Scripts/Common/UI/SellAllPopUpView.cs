using System;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsSignals;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Common.UI
{
    public class SellAllPopUpView : PopUpView
    {
        [SerializeField] private Button _sellBtn;
        public readonly TheSignal onClickSellAll = new();
        
        private void Awake()
        {
            _sellBtn.onClick.AddListener(() =>
            {
                onClickSellAll.Dispatch();
            });
        }
    }
}