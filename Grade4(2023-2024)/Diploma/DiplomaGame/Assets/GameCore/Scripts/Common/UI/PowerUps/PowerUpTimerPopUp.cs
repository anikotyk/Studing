using GameBasicsCore.Game.Views.UI.PopUps;
using TMPro;
using UnityEngine;

namespace GameCore.Common.UI.PowerUps
{
    public class PowerUpTimerPopUp : PopUpView
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        public TextMeshProUGUI timerText => _timerText;
    }
}
