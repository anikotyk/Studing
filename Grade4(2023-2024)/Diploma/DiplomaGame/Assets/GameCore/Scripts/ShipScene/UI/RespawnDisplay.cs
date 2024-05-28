using System;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.ShipScene.Battle;
using GameCore.ShipScene.Battle.Waves;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSignals.Api;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.ShipScene.UI
{
    public class RespawnDisplay : InjCoreMonoBehaviour
    {
        [SerializeField] private Button _respawnButton;
        [SerializeField] private CanvasGroup _content;
        [SerializeField] private TMP_Text _waveText;
        
        [Inject, UsedImplicitly] public BattleController battleController { get; }

        private List<ISignalCallback> _callbacks = new();

        private void Start()
        {
            Hide();
        }

        public override void Construct()
        {
            battleController.started.On(OnWaveStarted);
            
            _respawnButton.onClick.AddListener(OnRespawnButtonClick);
        }

        private void OnWaveStarted(Wave wave)
        {
            _waveText.text = $"Wave {wave.index+1}";
            OffCallbacks();
            _callbacks.Add(wave.failed.On(OnWaveFailed));
            _callbacks.Add(wave.finished.On(OnWaveFinished));
        }

        private void OnWaveFailed()
        {
            OffCallbacks();
            Show();
        }

        private void OnWaveFinished()
        {
            OffCallbacks();
            Hide();
        }

        private void OffCallbacks()
        {
            foreach (var callback in _callbacks)
                callback.Off();
            _callbacks.Clear();
        }

        private void OnRespawnButtonClick()
        {
            Hide();
            battleController.Restart();
        }

        private void Show()
        {
            DOTween.Kill(this);
            _content.alpha = 0.0f;
            _content.gameObject.SetActive(true);
            _content.DOFade(1, 0.5f).SetId(this);
        }

        private void Hide()
        {
            DOTween.Kill(this);
            _content.DOFade(0, 0.5f).OnComplete(() => _content.gameObject.SetActive(false)).SetId(this);
        }
    }
}