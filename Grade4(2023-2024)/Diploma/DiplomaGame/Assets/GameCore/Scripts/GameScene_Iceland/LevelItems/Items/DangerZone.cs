using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Factories;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Items
{
    public class DangerZone : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        [SerializeField] private AudioSource _dangerSound;
        private bool _isInZone;
        private UIDialogWindow _dangerWindow;
        
        public void OnEnterZone(BoatHuntingView view)
        {
            if (_isInZone) return;
            _isInZone = true;
           
            if (_dangerWindow != null)
            {
                _dangerWindow.dialog.transform.localScale = Vector3.one;
                _dangerWindow.overlayCanvasGroup.alpha = 1;
                _dangerWindow.gameObject.SetActive(true);
            }
            else
            {
                windowFactory.Create<UIDialogWindow>("DangerDialog", window =>
                {
                    if (_isInZone)
                    {
                        window.dialog.transform.localScale = Vector3.one;
                        window.overlayCanvasGroup.alpha = 1;
                        window.gameObject.SetActive(true);
                    }
                    _dangerWindow = window;
                });
            }
            
            _dangerSound.Play();
        }
        
        public void OnExitZone(BoatHuntingView view)
        {
            ExitZoneInternal();
        }

        public bool IsNowBoatInZone()
        {
            return _isInZone;
        }

        public void ExitZoneInternal()
        {
            _isInZone = false;
            if (_dangerWindow != null)
            {
                _dangerWindow.gameObject.SetActive(false);
            }
            
            _dangerSound.Stop();
        }
    }
}