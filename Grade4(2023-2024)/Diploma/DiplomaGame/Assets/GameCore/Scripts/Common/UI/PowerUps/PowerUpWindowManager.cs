using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Factories;
using GameBasicsCore.Game.Misc.Refs;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.UI.PowerUps
{
    public abstract class PowerUpWindowManager : ControllerInternal
    {
        [Inject, UsedImplicitly] public UIWindowFactory uiWindowFactory { get; }
        [Inject, UsedImplicitly] public CanvasesRef canvasesRef { get; }

        public UIDialogWindow dialog { get; private set; }

        public readonly TheSignal onShowDialog = new();
        public readonly TheSignal onHideDialog = new();

        public void ShowDialogWindow()
        {
            if (dialog != null)
            {
                InitAndShowWindow(dialog);
                return;
            }

            CreateDialog();
        }

        protected abstract void CreateDialog();
        
        public void HideDialogWindow()
        {
            if (dialog == null) return;
            dialog.Hide();
        }
        
        protected void InitAndShowWindow(UIDialogWindow window)
        {
            dialog = window;
            
            window.Show();
            onShowDialog.Dispatch();
            window.onHideComplete.Once(_ =>
            {
                onHideDialog.Dispatch();
                dialog = null;
            });
        }
    }
}
