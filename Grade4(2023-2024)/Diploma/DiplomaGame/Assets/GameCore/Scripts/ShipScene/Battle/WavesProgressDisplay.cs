using System.Linq;
using DG.Tweening;
using GameCore.ShipScene.Battle.Utilities;
using GameCore.ShipScene.Battle.Waves;
using UnityEngine;

namespace GameCore.ShipScene.Battle
{
    public class WavesProgressDisplay : BattleStartEndListener
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private SimpleSlider _progressSlider;
        
        public override void Construct()
        {
            base.Construct();
            Show();
        }

        protected override void OnBattleStarted(Wave wave)
        {
            Validate();
        }

        protected override void OnBattleEnded(Wave wave)
        {
            Validate();
        }

        private void Show()
        {
            Validate();
            DOTween.Kill(this);
            _canvasGroup.DOFade(1, _fadeDuration).SetId(this);
        }

        private void Validate()
        {
            _progressSlider.value = battleController.waves.Count(x => x.isFinished) / (float)battleController.waves.Count;
        }

        private void Hide()
        {
            DOTween.Kill(this);
            _canvasGroup.DOFade(0, _fadeDuration).SetId(this);
        }
    }
}