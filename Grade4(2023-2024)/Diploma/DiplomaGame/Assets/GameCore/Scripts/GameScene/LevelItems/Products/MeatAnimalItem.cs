using DG.Tweening;
using EPOOutline;
using GameCore.Common.Controllers;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene_Island.LevelItems.Animal;
using GameCore.GameScene.LevelItems.Managers;
using GameCore.GameScene.Settings;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSignals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameCore.GameScene.LevelItems.Products
{
    public class MeatAnimalItem : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SignalHub hub { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public ProductionController productionController { get; }
        
        [SerializeField] private ProductDataConfig _meatConfig;
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        [SerializeField] private Outlinable _outlinable;
        [ColorUsage(true, true)]
        [SerializeField] private Color _defaultColor;
        [ColorUsage(true, true)]
        [SerializeField] private Color _hitColor;
        [SerializeField] private AnimatorParameterApplier _animHit;
        [SerializeField] private Transform _popUpPoint;
        [SerializeField] private bool _isToOverrideYPos = true;
        [SerializeField] private float _spawnYPos = -0.025f;
        [SerializeField] private bool _isToOverrideCountMeat = false;
        [SerializeField] private int _countSpawn = 1;
        [SerializeField] private bool _isToResetAnimal = true;
        
        private bool _isEnabled = true;
        public bool isEnabled => _isEnabled;

        private Tween _popUpTween;

        private MeatAnimalInteractItem _interactItemCached;

        public MeatAnimalInteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<MeatAnimalInteractItem>();
                return _interactItemCached;
            }
        }
        
         
        private BoundsMB _boundsCached;
        public BoundsMB bounds
        {
            get
            {
                if (_boundsCached == null) _boundsCached = GetComponent<BoundsMB>();
                return _boundsCached;
            }
        }
        
        
        private int _countHits;
        private bool _isDead;
        public bool isDead => _isDead;

        private HealthBarPopUp _healthPopUp;
        
        public readonly TheSignal onDead = new();

        public void Activate()
        {
            _countHits = 0;
            _outlinable.enabled = true;
            _isEnabled = true;
            _isDead = false;
            SetPopUp();
            _healthPopUp.SetValue(1);
            OnActivate();
        }

        protected virtual void OnActivate()
        {
            
        }

        public void OnHit()
        {
            _countHits++;
            _isEnabled = false;
            
            _healthPopUp.SetValue(1 - (_countHits * 1f / GameplaySettings.def.sharksData.countHits));
            int cnt = Random.Range(1, GameplaySettings.def.sharksData.countMeatFromHitMax + 1);
            if (_isToOverrideCountMeat)
            {
                cnt = _countSpawn;
            }

            cnt = (int)(cnt * productionController.productionMultiplier);
            Vector3 spawnPos = transform.position + Vector3.up * 0.1f;
            if (_isToOverrideYPos)
            {
                spawnPos.y = _spawnYPos;
            }
            spawnProductsManager.SpawnBunchAtPoint(_meatConfig, spawnPos,
                cnt, GameplaySettings.def.sharksData.rangeJumpMeat);
            
            EffectOnHit();
            if (_countHits >= GameplaySettings.def.sharksData.countHits)
            {
                Die();
            }
        }

        protected virtual void EffectOnHit()
        {
            RedColorTween();
            if (_outlinable != null)
            {
                _outlinable.OutlineParameters.Color = _hitColor;
            }

            if (_animHit != null)
            {
                _animHit.Apply();
            }
            transform.DOPunchRotation(Vector3.right * 10f, 0.5f).OnComplete(() =>
            {
                if (!_isDead)
                {
                    _isEnabled = true;
                    if (_outlinable != null)
                    {
                        _outlinable.OutlineParameters.Color = _defaultColor;
                    }
                }
            }).SetLink(gameObject);
        }

        private void RedColorTween()
        {
            Material[] mats = _meshRenderer.materials;
            foreach (var mat in mats)
            {
                Color col = mat.color;
                mat.DOColor(Color.red, 0.15f).OnComplete(() =>
                {
                    mat.DOColor(col, 0.15f).SetLink(gameObject);
                }).SetLink(gameObject);
            }
        }
        
        private void Die()
        {
            HidePopUp();
            _isDead = true;
            _outlinable.enabled = false;
            OnDie();
            onDead.Dispatch();
            
            transform.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack).OnComplete(()=>
            {
                if (_isToResetAnimal)
                {
                    ResetAnimal();
                }
            }).SetLink(gameObject);
        }

        protected virtual void OnDie()
        {
            
        }

        protected virtual void ResetAnimal()
        {
            if (_outlinable != null)
            {
                _outlinable.OutlineParameters.Color = _defaultColor;
                _outlinable.enabled = false;
            }
        }
        
        private void SetPopUp()
        {
            if (_healthPopUp == null)
            {
                _healthPopUp = popUpsController.SpawnUnderMenu<HealthBarPopUp>("HealthBar");
                _healthPopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
                _healthPopUp.worldSpaceConverter.followWorldObject = _popUpPoint.transform;
            }
           
            _healthPopUp.transform.localScale = Vector3.zero;
            _healthPopUp.gameObject.SetActive(true);
            
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            _popUpTween = _healthPopUp.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).SetLink(gameObject);
        }
        
        private void HidePopUp()
        {
            if (_healthPopUp == null) return;
            
            if (_popUpTween != null)
            {
                _popUpTween.Kill();
            }
            
            var popUp = _healthPopUp;
            _popUpTween = popUp.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                popUp.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }
    }
}