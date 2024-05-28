using GameBasicsCore.Game.Views.UI.PopUps;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Gradient = UnityEngine.Gradient;

namespace GameCore.GameScene.UI
{
    public class HealthBarPopUp : PopUpView
    {
        [SerializeField] private Image _bar;
        [SerializeField] private Gradient2 _gradient;
        [SerializeField] private Gradient _color1;
        [SerializeField] private Gradient _color2;
        [SerializeField] private Gradient _color3;

        public void SetValue(float value)
        {
            _bar.fillAmount = value;
            if (value >= 1f)
            {
                _gradient.EffectGradient = _color1;
            } else if (value >= 0.6f)
            {
                _gradient.EffectGradient = _color2;
            }else
            {
                _gradient.EffectGradient = _color3;
            }
        }
    }
}