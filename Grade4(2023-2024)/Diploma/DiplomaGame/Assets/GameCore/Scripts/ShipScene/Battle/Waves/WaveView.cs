using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace GameCore.ShipScene.Battle.Waves
{
    public class WaveView : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _gameObjects;
        [SerializeField] private List<Transform> _zoomTargets;
        [SerializeField] private float _zoomDuration;
        [SerializeField] private float _delayDisable;

        private void OnEnable()
        {
            _gameObjects.ForEach(x => x.SetActive(false));
        }

        [Button()]
        public void Show()
        {
            DOTween.Kill(this);
            foreach (var zoomTarget in _zoomTargets)
                zoomTarget.localScale = Vector3.one;

            for (int i = 0; i < _gameObjects.Count; i++)
                _gameObjects[i].SetActive(true);
        }
        
        [Button()]
        public void Hide()
        {
            _zoomTargets.ForEach(
                x=>x.DOScale(0, _zoomDuration).SetEase(Ease.InBack).SetId(this));
            
            DOVirtual.DelayedCall(_delayDisable, 
                () => _gameObjects.ForEach(x => x.SetActive(false))).SetId(this);
        }
    }
}