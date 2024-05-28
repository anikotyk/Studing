using System;
using GameCore.Common.Controllers.PowerUps;
using GameCore.Common.Settings;
using TMPro;
using UnityEngine;

namespace GameCore.Common.UI.PowerUps
{
    public class PowerUpOffer : TimerOfferButton
    {
        [SerializeField] private TextMeshProUGUI _offerText;
        
        private bool _isNoTimer;

        private PowerUpController _controller;

        public void Init(PowerUpController controller)
        {
            _controller = controller;
            
            controller.onPowerUpSpawned.On(Appear);

            controller.onPowerUpGet.On(Disappear);
            controller.onPowerUpDestroyed.On(Disappear);
            controller.onPowerUpWaitDestroyUpdate.On((timeLeft) =>
            {
                TimeSpan timeUntilDisappear = TimeSpan.FromSeconds(timeLeft);
                timerText.text = timeUntilDisappear.ToString(@"mm\:ss");;
                if (timeUntilDisappear.TotalSeconds <= PowerUpsSettings.def.disableWarningTime && !isInWarningMode)
                {
                    TurnOnWarningMode();
                }
            });
            
            SetOfferText("");
            Init();
        }

        public void SetOfferText(string text)
        {
            if (_isNoTimer)
            {
                _offerText.text = "";
                timerText.text = text;
            }
            else
            {
                _offerText.text = text;
            }
        }

        public void SetNoTimer()
        {
            _isNoTimer = true;
            timerText.text = _offerText.text;
            _offerText.text = "";
        }
        
        public override void OnButtonClick()
        {
            base.OnButtonClick();
            
            if(_controller == null) return;
            _controller.OnOfferButtonClick();
        }
    }
}