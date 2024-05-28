using JetBrains.Annotations;
using GameBasicsCore.Game.Models.GameCurrencies;
using Zenject;

namespace GameCore.GameScene.UI
{
    public class UpgradePanelSC : UpgradePanel
    {
        [Inject, UsedImplicitly] public PlayerSoftCurrencyCollectModel softCurrencyCollectModel { get; }
        protected override GameCurrencyCollectModel gameCurrencyCollectModel => softCurrencyCollectModel;
    }
}