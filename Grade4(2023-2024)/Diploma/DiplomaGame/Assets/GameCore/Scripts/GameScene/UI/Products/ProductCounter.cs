using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.LevelItems.Character.Modules;
using JetBrains.Annotations;
using NaughtyAttributes;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.UI.Products
{
    public class ProductCounter : InjCoreMonoBehaviourWithValidate
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Transform _toScale;
        
        [SerializeField] private bool _useProductsGroup;
        public bool UseProductsGroup() => _useProductsGroup;
        [SerializeField, HideIf("UseProductsGroup")] private List<ProductDataConfig> _acceptableDataConfigs;
        [SerializeField, ShowIf("UseProductsGroup")] private ProductsGroupDataConfig _acceptableGroupDataConfigs;
        public List<ProductDataConfig> acceptableDataConfigs => _useProductsGroup ? _acceptableGroupDataConfigs.items : _acceptableDataConfigs;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        private List<string> _productIds;
        
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        public MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;

        public ProductsCarrier carrier => mainCharacterView.GetModule<StackModule>().carrier; 
        private Vector3 _scale;
        
        private Tween _scaleTween;

        private int _amount = 0;

        public override void Construct()
        {
            initializeInOrderController.Add(Initialize, 10000);
        }

        private void Initialize()
        {
            _scale = transform.localScale;
            _productIds = acceptableDataConfigs.Select(item => item.id).ToList();
            carrier.onChange.On(CountProducts);
            
            CountProducts();
        }

        private void CountProducts()
        {
            int cnt = _productIds.Sum(productId => carrier.Count(productId, false));
            if (cnt != _amount)
            {
                _amount = cnt;
                SetAmount(_amount);
            }

            if (cnt <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (carrier)
            {
                carrier.onChange.OffAll();
            }
        }

        public void SetAmount(int amount, bool isToAnimate = true)
        {
            _amount = amount;
            _text.text = amount+"";
            if (isToAnimate)
            {
                if (_scaleTween != null)
                {
                    _scaleTween.Kill();
                    _toScale.localScale = _scale;
                }
                _scaleTween = _toScale.DOPunchScale(Vector3.one * 0.05f, 0.25f).SetLink(gameObject);
            }
        }

        public void IncreaseAmount(int amount = 1, bool isToAnimate = true)
        {
            _amount += amount;
            _text.text = _amount+"";
            
            if (isToAnimate)
            {
                if (_scaleTween != null)
                {
                    _scaleTween.Kill();
                    _toScale.localScale = _scale;
                }
                _scaleTween = _toScale.DOPunchScale(Vector3.one * 0.1f, 0.25f).SetLink(gameObject);
            }
        }
    }
}