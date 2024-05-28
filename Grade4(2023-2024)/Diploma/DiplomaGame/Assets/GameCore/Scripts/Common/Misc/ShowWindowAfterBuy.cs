using DG.Tweening;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Factories;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Misc
{
    public class ShowWindowAfterBuy : ActionAfterBuy
    {
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        [InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }
        
        [SerializeField] private string _windowId;
       
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
            
            windowFactory.Create<UIDialogWindow>(_windowId, window =>
            {
                window.Show();
            }, false);
        }
    }
}