using System;
using DG.Tweening;
using GameCore.ShipScene.Battle.Utilities;
using GameCore.ShipScene.Battle.Waves;
using GameBasicsCore.Game.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.ShipScene.Battle
{
    public class BattleButton : BattleStartEndListener
    {
        [SerializeField] private Button _button;
        [SerializeField] private CanvasGroup _buttonCanvasGroup;
        [SerializeField] private float _fadeDuration;
        

        public override void Construct()
        {
            base.Construct();
            Show();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            battleController.StartBattle();
        }
        
        protected override void OnBattleStarted(Wave wave) => Hide();
        public void Hide()
        {
            DOTween.Kill(this);
            _buttonCanvasGroup.DOFade(0, _fadeDuration).SetId(this);
            _button.interactable = false;
        }
        
        
        protected override void OnBattleEnded(Wave wave) => Show();
        public void Show()
        {
            if(battleController.isStarted)
                return;
            DOTween.Kill(this);
            _buttonCanvasGroup.DOFade(1, _fadeDuration)
                .OnComplete(()=> _button.interactable = true)
                .SetId(this);
        }






    }
}