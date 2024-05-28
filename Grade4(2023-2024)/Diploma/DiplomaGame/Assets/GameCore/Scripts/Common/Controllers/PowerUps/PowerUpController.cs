using System.Threading.Tasks;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.PowerUps;
using GameCore.Common.UI;
using GameCore.Common.UI.PowerUps;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsCore.Game.Views.UI.Windows.Menus;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext.CamAttention;
using GameBasicsSignals;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace GameCore.Common.Controllers.PowerUps
{
    public abstract class PowerUpController : ControllerInternal
    {
        [Inject, UsedImplicitly] public GameplayUIMenuWindow gameplayUIMenuWindow { get; }
        [Inject, UsedImplicitly] public PowerUpsController powerUpsController { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [InjectOptional, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public PowerUpsLevelManager powerUpsLevelManager { get; }
        [Inject, UsedImplicitly] public CameraAttentionController cameraAttentionController { get; }
        [InjectOptional, UsedImplicitly] public IAdsRVRunner rvRunner { get; }
        [InjectOptional, UsedImplicitly] public IButtonUiClickSfxPlayer buttonUiClickSfxPlayer { get; }
        [Inject, UsedImplicitly] public PowerUpsOfferContainer powerUpsOfferContainer { get; }
        [Inject, UsedImplicitly] public PowerUpsCheatsController powerUpsCheatsController { get; }
        
        protected abstract PowerUpWindowManager powerUpWindowManager { get; }

        public abstract string cheatBtnName { get; }
        protected abstract bool powerUpEnabled { get; }
        protected abstract float powerUpMinFirstAppearDelay { get; }
        protected abstract float powerUpMaxFirstAppearDelay { get; }
        protected abstract float powerUpAppearInterval { get; }
        protected abstract float powerUpOfferTime { get; }
        protected abstract PowerUpContainer powerUpPrefab { get; }
        protected abstract BuyObject activateBuyObject { get; }
        protected virtual string savePropertyOnceClaimedName { get; }
        
        protected PowerUpContainer powerUp;
        
        protected Tween scheduleTween = null;
        protected Tween destroySuperPowerTween = null;
        
        private PowerUpOffer _powerUpOffer;
        public PowerUpOffer powerUpOffer => _powerUpOffer;
        
        private bool _isPaused;

        private bool _isSpawnEnabled = true;
        protected virtual bool isScheduleOnDestroy => true;
        
        protected TheSaveProperty<bool> _isOnceClaimed;
        
        public readonly TheSignal onPowerUpGet = new();
        public readonly TheSignal onPowerUpDestroyed = new();
        public readonly TheSignal<float> onPowerUpWaitDestroyUpdate = new();
        public readonly TheSignal onPowerUpSpawned = new();
        
        public override void Construct()
        {
            if (!powerUpEnabled) return;
            
            powerUpsCheatsController.AddController(this);

            initializeInOrderController.Add(Initialize, 1000);
            
            SpawnOffer();
            
            _isOnceClaimed = new(savePropertyOnceClaimedName, linkToDispose: level.gameObject);
        }
        
        private void Initialize()
        {
            if (!activateBuyObject) return;
            if (activateBuyObject.isBought)
            {
                OnPowerUpsAvailableForSpawn();
            }
            else
            {
                activateBuyObject.onBuy.Once(OnPowerUpsAvailableForSpawn);
            }
        }
        
        protected virtual void OnPowerUpsAvailableForSpawn()
        {
            ScheduleFirstAppearPowerUp();
        }

        public bool IsAvailableToSpawn()
        {
            return (rvRunner!=null && rvRunner.IsAvailable());
        }

        private void ScheduleFirstAppearPowerUp()
        {
            if (scheduleTween != null)
            {
                scheduleTween.Kill();
            }

            float delay = Random.Range(powerUpMinFirstAppearDelay, powerUpMaxFirstAppearDelay);
            scheduleTween = DOVirtual.DelayedCall(delay, AddPowerUpToSpawnQueue).SetLink(powerUpsLevelManager.gameObject);
        }
        
        protected void ScheduleAppearPowerUp()
        {
            if (scheduleTween != null)
            {
                scheduleTween.Kill();
            }
            scheduleTween = DOVirtual.DelayedCall(powerUpAppearInterval, AddPowerUpToSpawnQueue).SetLink(powerUpsLevelManager.gameObject);
        }

        private void AddPowerUpToSpawnQueue()
        {
            powerUpsController.AddToSpawnQueue(this);
        }

        public bool TrySpawnPowerUp()
        {
            if (IsAvailableToSpawn())
            {
                SpawnPowerUp();
                return true;
            }
            else
            {
                OnFailedSpawnPowerUp();
                return false;
            }
        }

        protected virtual void OnFailedSpawnPowerUp()
        {
            ScheduleAppearPowerUp();
        }

        protected virtual Vector3 GetPowerUpPosition()
        {
            return powerUpsLevelManager.GetPowerUpPosition();
        }
        public void SpawnPowerUp()
        {
            powerUp = Object.Instantiate(powerUpPrefab, powerUpsLevelManager.transform);
            Vector3 position = GetPowerUpPosition();
            
            powerUp.onRequestWindowShow.Off(ShowDialog);
            powerUp.onRequestWindowShow.On(ShowDialog);

            powerUp.onGetClicked.Off(OnGetPopUpClicked);
            powerUp.onGetClicked.On(OnGetPopUpClicked);
            
            powerUp.Init(position, popUpsController, this);
            
            SetPowerUpData(powerUp);
            SetGetPopUpData(powerUp.getPopUp);
            SetOffer();
            
            if (destroySuperPowerTween != null)
            {
                destroySuperPowerTween.Kill();
            }
            
            destroySuperPowerTween = DOVirtual.DelayedCall(powerUpOfferTime, DestroySuperPower, false)
                .OnUpdate(()=>
                {
                    float timeLeft = destroySuperPowerTween.Duration() *
                                     (1 - destroySuperPowerTween.ElapsedPercentage());
                    onPowerUpWaitDestroyUpdate.Dispatch(timeLeft);
                })
                .SetLink(powerUp.gameObject);

            if (IsToShowByCamera())
            {
                ShowByCamera();
            }
            
            onPowerUpSpawned.Dispatch();
        }
        
        private void SpawnOffer()
        {
            _powerUpOffer = powerUpsOfferContainer.SpawnOffer(this);
        }

        private void OnGetPopUpClicked()
        {
            buttonUiClickSfxPlayer?.Play();
        }
        
        protected abstract void SetOffer();

        protected virtual void SetPowerUpData(PowerUpContainer powerUp)
        {
            
        }
        
        protected abstract void SetGetPopUpData(GetBubblePopUpView getPopUp);

        protected virtual bool IsToShowByCamera()
        {
            return false;
        }
        
        public void OnOfferButtonClick()
        {
            buttonUiClickSfxPlayer?.Play();
            PauseDestroyPowerUp();
            cameraAttentionController.Run(powerUp.transform, 7)
                .SetMenuBlock(true)
                .SetCenterAngle(-6)
                .SetPriority(-1000)
                .OnFocus(OnFocusedAfterOfferButtonClick)
                .OnComplete(() => powerUp?.OnCameraAttentionEnded());
        } 
        
        private void OnFocusedAfterOfferButtonClick()
        {
            powerUp.OnCameraAttention();
            ShowDialog();
            powerUpWindowManager.onHideDialog.Once(() =>
            {
                if (cameraAttentionController.HasActive()) cameraAttentionController.StopActive();
            });
        }

        protected virtual void ShowByCamera()
        {
            if (!powerUp) return;
            var cameraAttention = cameraAttentionController.Run(powerUp.transform, 7)
                .SetMenuBlock(true)
                .SetCenterAngle(-6)
                .SetDuration(1f)
                .SetPriority(-1000);
        }
        
        private void DestroySuperPower()
        {
            if (powerUp != null)
            {
                powerUp.Hide();
                powerUp = null;

                if (isScheduleOnDestroy)
                {
                    ScheduleAppearPowerUp();
                }

                OnDestroyPowerUp();
                onPowerUpDestroyed.Dispatch();
            }
        }

        protected virtual void OnDestroyPowerUp()
        {
            
        }
        
        protected async void ShowDialog()
        {
            if(powerUp == null) return;
            PauseDestroyPowerUp();
               
            interactorCharactersCollection?.mainCharacterModel.DisableMovement(this);
            
            powerUp.TurnOffGetPopUp();
            powerUp.TurnOffTimer();
                
            await Task.Delay(150);

            interactorCharactersCollection?.mainCharacterModel.EnableMovement(this);

            SpawnDialog(powerUp);
        }
        
        protected abstract void SpawnDialog(PowerUpContainer powerUpContainer);

        protected void OnClaimedPowerUp()
        {
            if(powerUp == null) return;
            
            powerUp.OnClaimed();

            _isOnceClaimed.value = true;
        }
        
        protected void OnSkippedPowerUp(PowerUpContainer powerUpContainer)
        {
            powerUpContainer.Reset();
                            
            UnpauseDestroyPowerUp();
        }

        private void PauseDestroyPowerUp()
        {
            if(_isPaused) return;
            _isPaused = true;
            
            if (destroySuperPowerTween != null) 
            {
                destroySuperPowerTween.Pause();
            }
        }
        
        private void UnpauseDestroyPowerUp()
        {
            if(!_isPaused) return;
            _isPaused = false;
            
            if (destroySuperPowerTween != null)
            {
                destroySuperPowerTween.Play();
            }
        }

        protected virtual bool IsClaimPowerUpFree()
        {
            return false;
        }
        
        protected void GetPowerUp()
        {
            if (destroySuperPowerTween != null)
            {
                destroySuperPowerTween.Kill();
            }
            
            onPowerUpGet.Dispatch();

            OnGetPowerUp();
        }

        protected virtual void OnGetPowerUp()
        {
            ScheduleAppearPowerUp();
        }
        
        private void Deactivate()
        {
            if (powerUp != null)
            {
                powerUp.Hide();
                KillAllTweens();
            }
        }

        protected virtual void KillAllTweens()
        {
            if (scheduleTween != null)
            {
                scheduleTween.Kill();
            }
            if (destroySuperPowerTween != null)
            {
                destroySuperPowerTween.Kill();
            }
            
            DOTween.Kill(this);
        }

        public virtual void SpawnPowerUpCheat()
        {
            if (powerUp == null)
            {
                KillAllTweens();
                SpawnPowerUp();
            }
            else
            {
                Debug.LogWarning("This type of power up already exist on game scene");
            }
        }
    }
}
