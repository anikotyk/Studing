using System;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.GameScene.UI;
using GameCore.ShipScene.Battle;
using GameCore.ShipScene.Battle.Utilities;
using GameCore.ShipScene.Battle.Waves;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Factories;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.ShipScene.UI
{
    public class UpgradesButton : BattleStartEndListener
    {
        [SerializeField] private List<CanvasGroup> _groupsToHide;
        [SerializeField] private CanvasGroup _buttonGroup;
        [SerializeField] private Button _button;
        [SerializeField] private string _dialogWindowId;

        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        
        [InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }

        private UpgradesDialog _upgradesDialog;
        
        protected override void OnBattleStarted(Wave wave)
        {
            if (_upgradesDialog != null)
            {
                _upgradesDialog.Hide();
                _upgradesDialog = null;
            }

            HideButton();
        }

        protected override void OnBattleEnded(Wave wave)
        {
            ShowButton();
        }

        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            hapticService?.Selection();
            Hide();
            windowFactory.Create<UpgradesDialog>(_dialogWindowId, window =>
            {
                _upgradesDialog = window;
                window.onHideComplete.Once(_ =>
                {
                    Show();
                });
                window.Show();
            });
        }

        private void Hide()
        {
            DOTween.Kill(this);
            foreach (var group in _groupsToHide)
            {
                group.interactable = false;
                group.DOFade(0, 0.5f).SetId(this);
            }
            HideButton();
        }

        private void HideButton()
        {
            _buttonGroup.interactable = false;
            _buttonGroup.DOFade(0, 0.5f).SetId(this);
        }

        private void ShowButton()
        {
            _buttonGroup.interactable = true;
            _buttonGroup.DOFade(1, 0.5f).SetId(this);
        }

        private void Show()
        {
            DOTween.Kill(this);
            foreach (var group in _groupsToHide)
            {
                group.interactable = true;
                group.DOFade(1, 0.5f).SetId(this);
            }

            ShowButton();

        }
    }
}