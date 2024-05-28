using DG.Tweening;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems
{
    public class HealthModule : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        
        [SerializeField] private float _maxHealth = 3;
        [SerializeField] private Transform _popUpPoint;
        private float _currentHealth;
        private HealthBarPopUp _healthPopUp;
        private Tween _popUpTween;

        private bool _isPopUpActive;
        public bool isPopUpActive => _isPopUpActive;
        public TheSignal onDied { get; } = new();
        

        public void GetDamage(float multipler = 1)
        {
            _currentHealth -= multipler;
            _healthPopUp.SetValue(_currentHealth / _maxHealth);
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                onDied.Dispatch();
                HidePopUp();
            }
        }

        public void ResetHealth(bool isToShowHealthBar = true)
        {
            _currentHealth = _maxHealth;
            if (isToShowHealthBar)
            {
                ActivateHealth();
            }

            if (_healthPopUp)
            {
                _healthPopUp.SetValue(1);
            }
        }
        
        public void DeactivateHealth(bool isInternal = false)
        {
            HidePopUp(isInternal);
        }
        
        public void ActivateHealth(bool isInternal = false)
        {
            SetPopUp(isInternal);
        }
        
        private void SetPopUp(bool isInternal = false)
        {
            _isPopUpActive = true;
            if (_healthPopUp == null)
            {
                _healthPopUp = popUpsController.SpawnUnderMenu<HealthBarPopUp>("HealthBar");
                _healthPopUp.worldSpaceConverter.updateMethod = UpdateMethod.FixedUpdate;
                _healthPopUp.worldSpaceConverter.followWorldObject = _popUpPoint.transform;
            }
            
            _healthPopUp.gameObject.SetActive(true);
            
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }

            if (isInternal)
            {
                _healthPopUp.transform.localScale = Vector3.one;
            }
            else
            {
                _healthPopUp.transform.localScale = Vector3.zero;
                _popUpTween = _healthPopUp.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
            }
            _healthPopUp.SetValue(_currentHealth / _maxHealth);
        }

        private void HidePopUp(bool isInternal = false)
        {
            _isPopUpActive = false;
            if (_healthPopUp == null) return;
            
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            
            if (isInternal)
            {
                _healthPopUp.transform.localScale = Vector3.zero;
                _healthPopUp.gameObject.SetActive(false);
            }
            else
            {
                var popUp = _healthPopUp;
                _popUpTween = popUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
                {
                    popUp.gameObject.SetActive(false);
                }).SetLink(gameObject);
            }
        }
    }
}