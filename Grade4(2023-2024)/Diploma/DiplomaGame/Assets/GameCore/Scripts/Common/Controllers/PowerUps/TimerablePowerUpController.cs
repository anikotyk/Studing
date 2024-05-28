using System;
using DG.Tweening;
using GameCore.Common.Settings;
using GameCore.Common.UI.PowerUps;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Windows.Menus;
using GameBasicsSignals;
using Zenject;

namespace GameCore.Common.Controllers.PowerUps
{
    public abstract class TimerablePowerUpController : PowerUpController, ITimerablePowerUpController
    {
        [Inject, UsedImplicitly] public SuperPowerTimersContainer superPowerTimersContainer { get; }
        [Inject, UsedImplicitly] public GameplayUIMenuWindow gameplayUIMenuWindow { get; }

        protected abstract PowerUpsSettings.SuperPowerData superPowerData { get; }
        protected override bool powerUpEnabled => superPowerData.enabled;
        protected float powerUpWorkingTime => PowerUpsSettings.def.workingTime;
        protected override float powerUpMinFirstAppearDelay => superPowerData.config.minDelayFirstAppear;
        protected override float powerUpMaxFirstAppearDelay => superPowerData.config.maxDelayFirstAppear;
        protected override float powerUpAppearInterval => superPowerData.config.intervalAppear;
        protected override float powerUpOfferTime => PowerUpsSettings.def.offerTime;

        private DateTime _superPowerTurnOffTime;
        public TimeSpan timeUntilSuperPowerTurnOff => _superPowerTurnOffTime - DateTime.Now;
        
        private DateTime _superPowerPauseTime;
        private bool _isSuperPowerTurnedOn;

        private int _pause = 0;

        private Tween _endSuperPowerTween = null;

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
        
        public override void Construct()
        {
            base.Construct();
            
            hub.Get<NCSgnl.IUIWindowSignals.ShowStart>().On(window =>
            {
                if(!(window is GameplayUIMenuWindow)) PauseSuperPower();
            });
            hub.Get<NCSgnl.IUIWindowSignals.HideComplete>().On(window =>
            {
                if(!(window is GameplayUIMenuWindow)) UnpauseSuperPower();
            });
            
            gameplayUIMenuWindow.onHideStart.On(_ =>
            {
                PauseSuperPower();
            });
            
            gameplayUIMenuWindow.onShowStart.On(_ =>
            {
                UnpauseSuperPower();
            });

            SpawnTimer(); 
        }

        private void SpawnTimer()
        {
            superPowerTimersContainer.SpawnTimer(this, superPowerData.config.sprite);
        }
        
        protected override void OnGetPowerUp()
        {
            TurnOnSuperPowerEffect();
        }
        
        protected void TurnOnSuperPowerEffect()
        {
            _isSuperPowerTurnedOn = true;
            
            if (scheduleTween != null)
            {
                scheduleTween.Kill();
            }

            PowerEffect();
            
            //superPowerOnceClaimedSaveProperty.value = true;
            _pause = 0;

            _superPowerTurnOffTime = DateTime.Now.AddSeconds(powerUpWorkingTime);
            
            if (_endSuperPowerTween != null)
            {
                _endSuperPowerTween.Kill();
            }

            _endSuperPowerTween = DOVirtual.DelayedCall(powerUpWorkingTime,()=>
                {
                    TurnOffSuperPowerEffect();
                    ScheduleAppearPowerUp();
                })
                .OnUpdate(()=>
                {
                    onSuperPowerWaitEffectEndUpdate.Dispatch(_endSuperPowerTween.Duration() - _endSuperPowerTween.Elapsed());
                })
                .SetLink(powerUpsLevelManager.gameObject);

            onSuperPowerEffectStart.Dispatch();
        }

        protected virtual void PowerEffect()
        {
            
        }
        
        private void TurnOffSuperPowerEffect()
        {
            _isSuperPowerTurnedOn = false;

            EndPowerEffect();

            onSuperPowerEffectEnd.Dispatch();
        }
        
        protected virtual void EndPowerEffect()
        {
           
        }

        protected override bool IsClaimPowerUpFree()
        {
            return PowerUpsSettings.def.isFirstPowerUpFree && !_isOnceClaimed.value;
        }

        private void PauseSuperPower()
        {
            if (!_isSuperPowerTurnedOn) return;
            
            _pause++;
            if (_pause > 1) return;
            
            if (_endSuperPowerTween != null)
            {
                _endSuperPowerTween.Kill();
            }

            _superPowerPauseTime = DateTime.Now;
            onSuperPowerPause.Dispatch();
        }
        
        private void UnpauseSuperPower()
        {
            if (!_isSuperPowerTurnedOn) return;
            
            _pause--;
            if (_pause > 0) return;
            
            if (_endSuperPowerTween != null)
            {
                _endSuperPowerTween.Kill();
            }

            _superPowerTurnOffTime = _superPowerTurnOffTime.AddSeconds((DateTime.Now - _superPowerPauseTime).TotalSeconds);
            
            _endSuperPowerTween = DOVirtual.DelayedCall((float)timeUntilSuperPowerTurnOff.TotalSeconds,()=>
                {
                    TurnOffSuperPowerEffect();
                    ScheduleAppearPowerUp();
                })
                .OnUpdate(()=>
                {
                    onSuperPowerWaitEffectEndUpdate.Dispatch(_endSuperPowerTween.Duration() - _endSuperPowerTween.Elapsed());
                })
                .SetLink(powerUpsLevelManager.gameObject);
            
            onSuperPowerUnpause.Dispatch();
        }

        protected override void KillAllTweens()
        {
            base.KillAllTweens();
            
            if (_endSuperPowerTween != null)
            {
                _endSuperPowerTween.Kill();
            }
        }
    }
}
