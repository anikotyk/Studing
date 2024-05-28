using DG.Tweening;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

#pragma warning disable 0649
namespace GameCore.Common.UI
{
    public class CommonRightControls : InjCoreMonoBehaviour
    {
        [SerializeField] private CanvasGroup _buttonsCanvasGroup;

        [Inject, UsedImplicitly] public SignalHub hub { get; }
        
        private CanvasGroup _canvasGroupCached;
        public CanvasGroup canvasGroup => _canvasGroupCached ??= GetComponent<CanvasGroup>();

        private ActionPermissionAuditor _buttonsHideAuditor = new();

        public override void Construct()
        {
            hub.Get<NCSgnl.IUIWindowSignals.ShowStart>().On(HideButtons);
            hub.Get<NCSgnl.IUIWindowSignals.HideStart>().On(ShowButtons);
        }

        private void ShowButtons(object requestId)
        {
            _buttonsHideAuditor.Remove(requestId);
            
            if (_buttonsHideAuditor.count > 0) return;
            
            DOTween.Kill(_buttonsCanvasGroup);
            _buttonsCanvasGroup.blocksRaycasts = true;
            _buttonsCanvasGroup.gameObject.SetActive(true);
            _buttonsCanvasGroup.DOFade(1f, 0.5f).SetId(_buttonsCanvasGroup).SetLink(gameObject);
        }

        private void HideButtons(object requestId)
        {
            _buttonsHideAuditor.Add(requestId, true);
            if (_buttonsHideAuditor.count > 0 && _buttonsCanvasGroup.alpha > 0)
            {
                DOTween.Kill(_buttonsCanvasGroup);
                _buttonsCanvasGroup.blocksRaycasts = false;
                _buttonsCanvasGroup.DOFade(0f, 0.5f).SetId(_buttonsCanvasGroup).SetLink(gameObject)
                    .OnComplete(() => _buttonsCanvasGroup.gameObject.SetActive(false));
            }
        }
    }
}