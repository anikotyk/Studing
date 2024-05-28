using DG.Tweening;
using GameCore.ShipScene.Battle.Utilities;
using GameCore.ShipScene.Battle.Waves;
using TMPro;
using UnityEngine;

namespace GameCore.ShipScene.Battle
{
    public class BattleDisplay : BattleStartEndListener
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _waveDisplayText;
        [SerializeField] private bool _hideOnBattleEnd = true;
        
        public override void Construct()
        {
            base.Construct();
            if(_hideOnBattleEnd) Hide();
        }

        protected override void OnBattleStarted(Wave wave)
        {
            _waveDisplayText.text = $"Wave {wave.index + 1}/{battleController.waves.Count}";
            //_waveDisplayText.text = $"{wave.index + 1}/{battleController.waves.Count}";
            Show();
        }

        protected override void OnBattleEnded(Wave wave)
        {
            if(_hideOnBattleEnd) Hide();
        }


        private void Show()
        {
            DOTween.Kill(this);
            _canvasGroup.DOFade(1, 0.5f).SetId(this);
        }

        private void Hide()
        {
            DOTween.Kill(this);
            _canvasGroup.DOFade(0, 0.5f).SetId(this);
        }
    }
}