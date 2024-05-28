using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.Common.UI;
using GameCore.Common.LevelItems.PowerUps;
using GameCore.Common.Settings;
using GameCore.Common.Sounds.Api;
using GameCore.Common.UI.PowerUps;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc.Refs;
using GameBasicsCore.Game.Models.GameCurrencies;
using GameBasicsCore.Game.Presenters.Currencies;
using GameBasicsCore.Game.SaveProperties.Api;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Controllers.PowerUps
{
    public class StackOfCashController : PowerUpController
    {
        [Inject, UsedImplicitly] public MainCameraRef mainCameraRef { get; }
        [InjectOptional, UsedImplicitly] public SoftCurrencyPresenter softCurrencyPresenter { get; }
        [InjectOptional, UsedImplicitly] public PlayerSoftCurrencyCollectModel softCurrencyCollectModel { get; }
        [Inject, UsedImplicitly] public CurrencyPowerUpWindowManager windowManager { get; }
        [InjectOptional, UsedImplicitly] public StackOfCashLevelManager stackOfCashLevelManager { get; }
        [InjectOptional, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [InjectOptional, UsedImplicitly] public ISoftCurrencyUISfxPlayer softCurrencyUISfxPlayer { get; }
        
        protected override PowerUpWindowManager powerUpWindowManager => windowManager;
        protected string savePropertyOnceShownName => "OnceShownCoinsChest";
        protected override string savePropertyOnceClaimedName => "OnceClaimedCoinsChest";
        public override string cheatBtnName => "CoinsChest";
        protected override bool powerUpEnabled => PowerUpsSettings.def.stackOfCashData.enableStackOfCash;
        protected override float powerUpMinFirstAppearDelay => PowerUpsSettings.def.stackOfCashData.minDelayFirstAppear;
        protected override float powerUpMaxFirstAppearDelay => PowerUpsSettings.def.stackOfCashData.maxDelayFirstAppear;
        protected override float powerUpAppearInterval=> PowerUpsSettings.def.stackOfCashData.intervalAppear;
        protected override float powerUpOfferTime => PowerUpsSettings.def.offerTime;
        protected override PowerUpContainer powerUpPrefab => PowerUpsSettings.def.stackOfCashData.stackOfCashPrefab;
        protected override BuyObject activateBuyObject => powerUpsLevelManager.stackOfCashAvailable;

        protected TheSaveProperty<bool> _isOnceShown;
        
        private float _multiplierTaskPayAmount => 0.8f;

        private int _payment;

        public override void Construct()
        {
            base.Construct();
            
            _isOnceShown = new(savePropertyOnceShownName, linkToDispose: level.gameObject);
        }

        protected override void SetPowerUpData(PowerUpContainer powerUp)
        {
            base.SetPowerUpData(powerUp);
            
            _payment = GetCurrentPayment();
            powerUp.SetTextOnModel("<sprite=\"sc\" name=\"00\">" + _payment);
        }
        
        protected override void SetOffer()
        {
            powerUpOffer.SetIcon(PowerUpsSettings.def.stackOfCashData.icon);
            //powerUpOffer.SetOfferText(softCurrencyModel.config.iconCodeTMP + GetCurrentPayment());
        }

        
        private int GetCurrentPayment()
        {
            int payment = GetPaymentBasedOnCurrentTask();
            if (payment <= 0) payment = GetPaymentBasedOnPreviousTasks();
            if (payment <= 0) payment = GetPaymentBasedOnNextTasks();
            if (payment <= 0) payment = 20; //min possible value

            if (payment % 5 > 0)
            {
                payment += 5 - (payment % 5);
            }

            return payment;
        }
        
        private int GetPaymentBasedOnCurrentTask()
        {
            if (buyObjectsManager.currentBuyObject is DonateBuyObject donateBuyObject)
            {
                return (int)(donateBuyObject.buyPlatform.priceSoftCurrency * _multiplierTaskPayAmount);
            }

            return -1;
        }
        
        private int GetPaymentBasedOnPreviousTasks()
        {
            for (int i = buyObjectsManager.activeBuyObjectIndexSaveProperty.value - 1; i >= 0; i--)
            {
                var buyObject = buyObjectsManager.buyObjects[i];
                if (buyObject is DonateBuyObject donateBuyObject)
                {
                    if (donateBuyObject.buyPlatform.priceSoftCurrency > 0)
                    {
                        return (int)(donateBuyObject.buyPlatform.priceSoftCurrency * _multiplierTaskPayAmount);
                    }
                }
            }
           
            return -1;
        }
        
        private int GetPaymentBasedOnNextTasks()
        {
            for (int i = buyObjectsManager.activeBuyObjectIndexSaveProperty.value + 1; 
                 i < buyObjectsManager.buyObjects.Count; i++)
            {
                var buyObject = buyObjectsManager.buyObjects[i];
                if (buyObject is DonateBuyObject donateBuyObject)
                {
                    if (donateBuyObject.buyPlatform.priceSoftCurrency > 0)
                    {
                        return (int)(donateBuyObject.buyPlatform.priceSoftCurrency * _multiplierTaskPayAmount);
                    }
                }
            }
           
            return -1;
        }
        
        protected override Vector3 GetPowerUpPosition()
        {
            if (stackOfCashLevelManager) return stackOfCashLevelManager.GetPosition();
            return base.GetPowerUpPosition();
        }

        protected override void SetGetPopUpData(GetBubblePopUpView getPopUp)
        {
            getPopUp.SetTitle("Chest of Coins");
            getPopUp.SetDesc("<sprite=\"sc\" name=\"00\">" + _payment);
        }
        
        protected override bool IsToShowByCamera()
        {
            return !_isOnceShown.value;
        }
        
        protected override void ShowByCamera()
        {
            base.ShowByCamera();

            _isOnceShown.value = true;
        }
        
        protected override void OnGetPowerUp()
        {
            base.OnGetPowerUp();
            
            int cntParts = _payment;
            int maxCntParts = 50;
            cntParts = cntParts > maxCntParts ? maxCntParts : cntParts;
            int amount = _payment / cntParts;
            for (int i = 0; i < cntParts; i++)
            {
                softCurrencyPresenter.SpawnAndFlyToCollectDisplay(powerUp.transform.position, onComplete: () =>
                {
                    softCurrencyCollectModel.Earn(amount, "ChestRv", "ChestRv", true);
                    softCurrencyUISfxPlayer?.PlayCollected();
                }, radius: 0.5f, camera:mainCameraRef);
            }
            
            powerUp = null;
        }

        protected override void SpawnDialog(PowerUpContainer powerUpContainer)
        {
            windowManager.ShowDialogWindow();
            
            bool isClaimedPowerUp = false;
            windowManager.onShowDialog.Once(() =>
            {
                windowManager.currencyPowerUpDialog.SetRvTktPlacementName(CommStr.Rv_ClaimCoinsChest);
                windowManager.currencyPowerUpDialog.SetTitle("CHEST OF COINS");
                windowManager.currencyPowerUpDialog.SetAmountCurrency(_payment, CurrencyPowerUpDialog.CurrencyType.SoftCurrency);
                windowManager.currencyPowerUpDialog.SetIsFree(IsClaimPowerUpFree());
                windowManager.currencyPowerUpDialog.onClaim.Once(() =>
                {
                    isClaimedPowerUp = true;
                });
                windowManager.currencyPowerUpDialog.onSkip.Once(() =>
                {
                    OnSkippedPowerUp(powerUpContainer);
                });
            });

            windowManager.onHideDialog.Once(() =>
            {
                if (isClaimedPowerUp)
                {
                    OnClaimedPowerUp();
                    DOVirtual.DelayedCall(0.25f, GetPowerUp, false).SetLink(powerUpsLevelManager.gameObject);
                }
            });
        }
        
        protected override bool IsClaimPowerUpFree()
        {
            return PowerUpsSettings.def.isFirstPowerUpFree 
                   && PowerUpsSettings.def.stackOfCashData.isFirstFree 
                   && !_isOnceClaimed.value;
        }
    }
}
