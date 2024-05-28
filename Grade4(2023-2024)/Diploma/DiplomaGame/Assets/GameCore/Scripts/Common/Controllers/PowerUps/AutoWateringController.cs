using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using GameCore.Common.LevelItems.Helper;
using GameCore.Common.Misc;
using GameCore.Common.Settings;
using GameCore.Common.UI.PowerUps;
using GameCore.GameScene.LevelItems.Products;
using GameCore.GameScene.LevelItems.Tutorials;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Controllers;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext.CamAttention;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Controllers.PowerUps
{
    public class AutoWateringController : ControllerInternal, ITimerablePowerUpController
    {
        [Inject, UsedImplicitly] public SuperPowerTimersContainer superPowerTimersContainer { get; }
        [Inject, UsedImplicitly] public SuperPowerWindowManager windowManager { get; }
        [InjectOptional, UsedImplicitly] public IAdsRVRunner rvRunner { get; }
        [InjectOptional, UsedImplicitly] public AutoWateringOffer autoWateringOffer { get; }
        [InjectOptional, UsedImplicitly] public IButtonUiClickSfxPlayer buttonUiClickSfxPlayer { get; }
        [InjectOptional, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        [InjectOptional, UsedImplicitly] public ICheatPanelGUI cheatPanelGUI { get; }
        [Inject, UsedImplicitly] public CameraAttentionController cameraAttentionController { get; }
        protected PowerUpsSettings.SuperPowerData superPowerData =>
            PowerUpsSettings.def.wateringSuperPowerData;
        
        private List<WateringCharacterView> _characters = new List<WateringCharacterView>();

        private bool _isEffectWorking;

        private WaterFilterTutorial _waterFilterTutorial;
        private Tween _endSuperPowerTween = null;
        
        private DateTime _superPowerTurnOffTime;
        public TimeSpan timeUntilSuperPowerTurnOff => _superPowerTurnOffTime - DateTime.Now;
        
        private readonly TheSignal _onSuperPowerEffectStart = new();
        public TheSignal onSuperPowerEffectStart => _onSuperPowerEffectStart;
        
        private readonly TheSignal _onSuperPowerEffectEnd = new();
        public TheSignal onSuperPowerEffectEnd => _onSuperPowerEffectEnd;
        
        private readonly TheSignal<float> _onSuperPowerWaitEffectEndUpdate = new();
        public TheSignal<float> onSuperPowerWaitEffectEndUpdate => _onSuperPowerWaitEffectEndUpdate;
        
        private readonly TheSignal _onSuperPowerPause = new();
        public TheSignal onSuperPowerPause => _onSuperPowerPause;
        private readonly TheSignal _onSuperPowerUnpause = new();
        public TheSignal onSuperPowerUnpause => _onSuperPowerUnpause;
        
        private Tween _offerEndTween = null;
        private DateTime _lastTimeDeactivated;
        private bool _isPaused;
        private bool _isOffering;

        public readonly TheSignal onGet = new();
        public readonly TheSignal onOfferEnd = new();
        public readonly TheSignal<float> onWaitOfferEndUpdate = new();
        public readonly TheSignal onRequest = new();
        
        public override void Construct()
        {
            if (!superPowerData.enabled) return;
            if (!autoWateringOffer) return;
            
            autoWateringOffer.Init(this);
            
            _waterFilterTutorial = GameObject.FindObjectOfType<WaterFilterTutorial>(true);
            
            foreach (var waterFilter in GameObject.FindObjectsOfType<WaterFilterObject>(true))
            {
                waterFilter.onNeedsWater.On(() =>
                {
                    if (!_waterFilterTutorial || _waterFilterTutorial.isPassed)
                    {
                        if (!_isEffectWorking && IsAvailableToSpawn())
                        {
                            RequestOffer();
                        }
                    }
                });
            }
            
            SpawnTimer(); 
            
            cheatPanelGUI.onDraw.On(DrawCheats).Priority(1);
        }
        
        public override void Dispose()
        {
            base.Dispose();
            cheatPanelGUI.onDraw.Off(DrawCheats);
        }

        private void RequestOffer()
        {
            onRequest.Dispatch();

            _isOffering = true;

            if (_offerEndTween != null)
            {
                _offerEndTween.Kill();
            }
            _offerEndTween = DOVirtual.DelayedCall(PowerUpsSettings.def.offerTime, EndOffer, false)
                .OnUpdate(()=>
                {
                    float timeLeft = _offerEndTween.Duration() *
                                     (1 - _offerEndTween.ElapsedPercentage());
                    onWaitOfferEndUpdate.Dispatch(timeLeft);
                })
                .SetLink(level.gameObject);
        }

        private void EndOffer()
        {
            if (_offerEndTween != null)
            {
                _offerEndTween.Kill();
            }
            _isOffering = false;
            _lastTimeDeactivated = DateTime.Now;
            onOfferEnd.Dispatch();
        }
        
        private void SpawnTimer()
        {
            superPowerTimersContainer.SpawnTimer(this, superPowerData.config.sprite);
        }

        private void PowerEffect()
        {
            _isEffectWorking = true;
            if (_characters.Count <= 0)
            {
                SetupCharacters();
            }
            
            foreach (var character in _characters)
            {
                character.autoWateringLogicModule.Enable();
            }
            
            _superPowerTurnOffTime = DateTime.Now.AddSeconds(PowerUpsSettings.def.workingTime);

            _endSuperPowerTween = DOVirtual.DelayedCall(PowerUpsSettings.def.workingTime, EndPowerEffect, false).OnUpdate(()=>
            {
                onSuperPowerWaitEffectEndUpdate.Dispatch(_endSuperPowerTween.Duration() - _endSuperPowerTween.Elapsed());
            }).SetLink(level.gameObject);
            
            onSuperPowerEffectStart.Dispatch();
            _lastTimeDeactivated = DateTime.Now;
        }

        private void SetupCharacters()
        {
            foreach (var filter in GameObject.FindObjectsOfType<WaterFilterObject>(true))
            {
                var character = GameObject.Instantiate(PowerUpsSettings.def.wateringSuperPowerData.characterPrefab);
                _characters.Add(character);
                character.autoWateringLogicModule.Initialize(filter);
                character.gameObject.SetActive(false);
            }
        }
        
        private void EndPowerEffect()
        {
            _isEffectWorking = false;
            foreach (var character in _characters)
            {
                character.autoWateringLogicModule.Disable();
                character.gameObject.SetActive(false);
            }
            
            onSuperPowerEffectEnd.Dispatch();
        }

        public bool IsAvailableToSpawn()
        {
            var wateringWheel = GameObject.FindObjectOfType<WaterFilterObject>();
            return (rvRunner!=null && rvRunner.IsAvailable()) && wateringWheel != null
                   && (DateTime.Now - _lastTimeDeactivated).TotalSeconds > superPowerData.config.intervalAppear;
        }
        
        private void PauseDestroyPowerUp()
        {
            if(_isPaused) return;
            _isPaused = true;
            
            if (_offerEndTween != null) 
            {
                _offerEndTween.Pause();
            }
        }
        
        private void UnpauseDestroyPowerUp()
        {
            if(!_isPaused) return;
            _isPaused = false;
            
            if (_offerEndTween != null)
            {
                _offerEndTween.Play();
            }
        }

        public void OnOfferButtonClick()
        {
            buttonUiClickSfxPlayer?.Play();
            PauseDestroyPowerUp();
            
            var wateringWheel = GameObject.FindObjectOfType<WaterFilterObject>();
            if (wateringWheel == null)
            {
                UnpauseDestroyPowerUp();
                return;
            }
            cameraAttentionController.Run(wateringWheel.transform, 7)
                .SetMenuBlock(true)
                .SetCenterAngle(-6)
                .SetPriority(-1000)
                .OnFocus(OnFocusedAfterOfferButtonClick);
        }
        
        private void OnFocusedAfterOfferButtonClick()
        {
            ShowDialog();
            windowManager.onHideDialog.Once(() =>
            {
                if (cameraAttentionController.HasActive()) cameraAttentionController.StopActive();
            });
        }

        private void OnSkipped()
        { 
            UnpauseDestroyPowerUp();
        }
        
        protected async void ShowDialog()
        {
            interactorCharactersCollection?.mainCharacterModel.DisableMovement(this);
            
            await Task.Delay(150);

            interactorCharactersCollection?.mainCharacterModel.EnableMovement(this);

            SpawnDialog();
        }
        
        private void SpawnDialog()
        {
            windowManager.ShowDialogWindow();
            
            bool isClaimedPowerUp = false;
            windowManager.onShowDialog.Once(() =>
            {
                windowManager.superPowerDialog.SetIsFreeClaim(false);
                var description = superPowerData.config.description;
                TimeSpan timeSpan = TimeSpan.FromSeconds(PowerUpsSettings.def.workingTime);
                string formattedTime = string.Format("{0}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
                if (timeSpan.Seconds <= 0)
                {
                    formattedTime = timeSpan.Minutes.ToString();
                }
                description += " for "+ formattedTime +" min";
                windowManager.superPowerDialog.SetData(superPowerData.config.title, description, 
                    superPowerData.config.modelToWindow, CommStr.Rv_ClaimAutoWatering);
                windowManager.superPowerDialog.onClaimSuperPower.Once(() =>
                {
                    isClaimedPowerUp = true;
                });
                windowManager.superPowerDialog.onSkipSuperPower.Once(() =>
                {
                    OnSkipped();
                });
            });

            windowManager.onHideDialog.Once(() =>
            {
                if (isClaimedPowerUp)
                {
                    EndOffer();
                    PowerEffect();
                }
            });
        }
        
        private void DrawCheats(float width, float height)
        {
            GUILayout.BeginVertical("Box", GUILayout.Width(width));
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Auto watering"))
            {
                if (!_isEffectWorking && !_isOffering && GameObject.FindObjectOfType<WaterFilterObject>() != null)
                {
                    RequestOffer();
                }
                else
                {
                    Debug.LogWarning("AutoWatering already working or offering or no water filter");
                }
            }
            
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}