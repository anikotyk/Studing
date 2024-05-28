using System.Collections.Generic;
using Creatives.Scripts;
using DG.Tweening;
using GameCore.Common.Controllers;
using GameCore.Common.LevelItems.Items.HittableItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Animals.FightAnimal.Modules
{
    public class FightAnimalGetDamageModule : InjCoreMonoBehaviour, IHittable
    {
        [Inject, UsedImplicitly] public VfxStack vfxStack { get; }
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public ProductionController productionController { get; }
        
        [SerializeField] private ProductDataConfig _productSpawnConfig;
        [SerializeField] private ParticleSystem _vfxHit;
        [SerializeField] private int _spawnCount;
        [SerializeField] private float _productsJumpRange = 0.5f;
        [SerializeField] private AnimatorParameterApplier _dieAnim;
        [SerializeField] private MaterialBlinker _materialBlinker;
        [SerializeField] private BoxCollider _interactCollider;
        private bool _isEnabled = true;
        public bool isEnabled => _isEnabled;
        private List<SellProduct> _spawnedProducts = new List<SellProduct>();
        public List<SellProduct> spawnedProducts => _spawnedProducts;

        public virtual float canHitAngle => 60f;

        private FightAnimalView _animalViewCached;
        public FightAnimalView animalView => _animalViewCached ??= GetComponentInParent<FightAnimalView>(true);
        public CharacterTools.HittingToolType toolType => CharacterTools.HittingToolType.Spear;
        public Transform view => animalView.transform;
        public BoxCollider colliderInteract => _interactCollider;
        public Vector3 helperPosition => view.position + Vector3.back*0.5f;
        private Tween _punchTween;

        private bool _colorTweenRunning;

        private readonly TheSignal _onDie = new();
        public TheSignal onTurnOff => _onDie;
        public TheSignal onTurnOn => animalView.onActivate;

        public override void Construct()
        {
            base.Construct();
            animalView.healthAnimalModule.onDied.On(OnDied);
        }

        private void GetDamage(float multipler = 1)
        {
            animalView.healthAnimalModule.GetDamage(multipler);
        }

        public void OnHit(float multipler = 1)
        {
            GetDamage(multipler);
            _vfxHit.Play();
            DamageEffects();
            if (_punchTween != null)
            {
                _punchTween.Kill();
                animalView.animator.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
            _punchTween = animalView.animator.transform.DOPunchRotation(Vector3.right * 10f, 0.5f).SetLink(gameObject);
        }
        
        private void DamageEffects()
        {
            _materialBlinker.StartEffect();
        }

        private void OnDied()
        {
            _isEnabled = false;
            _dieAnim.Apply();
            onTurnOff.Dispatch();
            DOVirtual.DelayedCall(0.9f, () =>
            {
                animalView.Deactivate();
                vfxStack.Spawn(CommStr.DeathAnimalVFX, view.transform.position + Vector3.up * 0.25f);
                SpawnProducts();
            }, false).SetLink(gameObject);
        }

        private void SpawnProducts()
        {
            int spawnCnt = (int)(_spawnCount * productionController.productionMultiplier);
            var products = spawnProductsManager.SpawnBunchAtPoint(_productSpawnConfig, view.position + Vector3.up * 0.1f,
                spawnCnt, _productsJumpRange);
            foreach (var prod in products)
            {
                if (prod is SellProduct)
                {
                    var sellprod = (prod as SellProduct);
                    _spawnedProducts.Add(sellprod);
                    sellprod.onAddedToCarrier.Once(()=>
                    {
                        _spawnedProducts.Remove(sellprod);
                    });
                }
            }
        }

        public void ResetModule()
        {
            _isEnabled = true;
        }
    }
}