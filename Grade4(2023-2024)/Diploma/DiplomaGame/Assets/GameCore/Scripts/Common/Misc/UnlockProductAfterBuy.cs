using DG.Tweening;
using GameCore.GameScene.DataConfigs;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Factories;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Misc
{
    public class UnlockProductAfterBuy : ActionAfterBuy, IWindowShowable
    {
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        [InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }
        
        [SerializeField] private string _productName;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Sprite _iconOutline;
        [SerializeField] private ProductPriceDataConfig _priceDC;
        
        private readonly TheSignal _onWindowClosed = new();
        public TheSignal onWindowClosed => _onWindowClosed;
        
        protected override void Action()
        {
            base.Action();
            
            DOVirtual.DelayedCall(0.35f, () =>
            {
                ShowWindow();
            },false).SetLink(gameObject);
        }

        private void ShowWindow()
        {
            hapticService?.Selection();
            
            windowFactory.Create<UnlockNewProductDialog>("UnlockNewProductDialog", window =>
            {
                window.Initialize(_productName, _priceDC.softCurrencyCount, _icon, _iconOutline);
                window.Show();
                window.onHideComplete.Once((_) =>
                {
                    onWindowClosed.Dispatch();
                });
            }, false);
        }
    }
}