using System;
using GameCore.Common.LevelItems.PowerUps;
using GameCore.Common.UI;
using GameCore.Common.UI.PowerUps;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using Zenject;

namespace GameCore.Common.Controllers.PowerUps
{
    public abstract class SuperPowerController : TimerablePowerUpController
    {
        [Inject, UsedImplicitly] public VfxStack vfxStack { get; }
        [Inject, UsedImplicitly] public SuperPowerWindowManager windowManager { get; }
        protected override PowerUpContainer powerUpPrefab => superPowerData.config.superPowerPrefab;
        protected abstract string savePropertyOnceShownName { get; }
        private TheSaveProperty<bool> _isOnceShown;
        
        protected override PowerUpWindowManager powerUpWindowManager => windowManager;
        
        protected abstract string rvPlacementName { get; }
        protected virtual string textOnModel => superPowerData.config.shortDescription;

        public override void Construct()
        {
            base.Construct();
            
            _isOnceShown = new(savePropertyOnceShownName, linkToDispose: level.gameObject);
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

        protected override void SetGetPopUpData(GetBubblePopUpView getPopUp)
        {
            getPopUp.SetTitle(superPowerData.config.title);
            getPopUp.SetDesc(superPowerData.config.description);
        }
        
        protected override void SetPowerUpData(PowerUpContainer powerUp)
        {
            base.SetPowerUpData(powerUp);
            
            if (IsClaimPowerUpFree())
            {
                powerUp.SetTextOnModel("FREE");
            }
            else
            {
                powerUp.SetTextOnModel(textOnModel);
            }
        }
        
        protected override void SetOffer()
        {
            powerUpOffer.SetIcon(superPowerData.config.sprite);
            TimeSpan timeSpan = TimeSpan.FromSeconds(powerUpWorkingTime);
            string formattedTime = string.Format("{0}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
            if (timeSpan.Seconds <= 0)
            {
                formattedTime = timeSpan.Minutes.ToString();
            }
            powerUpOffer.SetOfferText(formattedTime +" min");
        }

        protected override void SpawnDialog(PowerUpContainer powerUpContainer)
        {
            windowManager.ShowDialogWindow();
            
            bool isClaimedPowerUp = false;
            windowManager.onShowDialog.Once(() =>
            {
                windowManager.superPowerDialog.SetIsFreeClaim(IsClaimPowerUpFree());
                var description = superPowerData.config.description;
                TimeSpan timeSpan = TimeSpan.FromSeconds(powerUpWorkingTime);
                string formattedTime = string.Format("{0}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
                if (timeSpan.Seconds <= 0)
                {
                    formattedTime = timeSpan.Minutes.ToString();
                }
                description += " for "+ formattedTime +" min";
                windowManager.superPowerDialog.SetData(superPowerData.config.title, description, 
                    superPowerData.config.modelToWindow, rvPlacementName);
                windowManager.superPowerDialog.onClaimSuperPower.Once(() =>
                {
                    isClaimedPowerUp = true;
                    OnClaimedPowerUp();
                });
                windowManager.superPowerDialog.onSkipSuperPower.Once(() =>
                {
                    OnSkippedPowerUp(powerUpContainer);
                });
            });

            windowManager.onHideDialog.Once(() =>
            {
                if (isClaimedPowerUp)
                {
                    GetPowerUp();
                }
            });
        }

    }
}
