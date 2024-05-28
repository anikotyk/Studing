using DG.Tweening;
using GameCore.Common.Misc;
using GameCore.Common.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Models;
using GameBasicsCore.Game.Models.GameCurrencies;
using GameBasicsCore.Game.Presenters;
using GameBasicsCore.Game.Settings.GameCore;
using GameBasicsCore.Game.Views.UI.Controls;
using GameBasicsCore.Tools;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.GameScene.UI
{
    public abstract class UpgradePanel : CoreMonoBehaviour
    {
        [SerializeField] private UpgradePropertyDataConfig _config;
        [SerializeField] private RvTktButtonsPanel _rvTktButtonsPanel;
        [SerializeField] private ButtonLabelDataPanel _btn;
        [SerializeField] private Image _imgProgress;

        [Inject, UsedImplicitly] public RvTktUIControlsPresenter rvTktUIControlsPresenter { get; }
        [Inject, UsedImplicitly] public UpgradesController upgradesController { get; }
        [InjectOptional, UsedImplicitly] public IButtonUiClickSfxPlayer buttonUiClickSfxPlayer { get; }
        protected virtual GameCurrencyCollectModel gameCurrencyCollectModel => null;
        protected UpgradePropertyModel model;
        
        private void Awake()
        {
            model = upgradesController.GetModel(_config);
            _rvTktButtonsPanel.Init(rvTktUIControlsPresenter);
            _rvTktButtonsPanel.SetPlacementName(CommStr.Rv_UpgradeCharacter);
            
            Validate(false);
            
            _rvTktButtonsPanel.onSuccess.On(() =>
            {
                model.TryUpgrade();
                Validate(true);
            });
            _rvTktButtonsPanel.onClick.On(() =>
            {
                buttonUiClickSfxPlayer?.Play();
            });
            _btn.btn.onClick.AddListener(() =>
            {
                gameCurrencyCollectModel.Use(model.price, NCStr.Res_Upgrade, _config.id);
                model.TryUpgrade();
                Validate(true);
                buttonUiClickSfxPlayer?.Play();
            });
            gameCurrencyCollectModel.onChange.On(_ => ValidateButton()).OffWhen(() => isDestroyed, false);
        }

        protected virtual void Validate(bool animate)
        {
            DOTween.Kill(this);

            var price = model.priceInt;
            var progress = (float)model.level / model.maxLevel;
            var isMax = model.level >= model.maxLevel;

            if (animate)
            {
                _imgProgress.DOFillAmount(progress, 0.2f).SetLink(gameObject).SetId(this);
            }
            else
            {
                _imgProgress.fillAmount = progress;
            }

            ValidateButton();
            if (isMax)
            {
                _btn.SetLabel("MAX");
            }
            else
            {
                _btn.SetLabel($"{GameCoreNewSettings.def.softCurrency.iconCodeTMP} {price.ToIdleFormat()}");
            }
        }

        private void ValidateButton()
        {
            var price = model.priceInt;
            var isMax = model.level >= model.maxLevel;
            _btn.btn.interactable = gameCurrencyCollectModel.total >= price && !isMax;
            
            if (isMax)
            {
                _rvTktButtonsPanel.gameObject.SetActive(false);
            }
        }
    }
}