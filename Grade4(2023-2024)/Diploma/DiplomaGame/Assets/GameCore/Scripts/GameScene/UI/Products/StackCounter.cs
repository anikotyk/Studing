using DG.Tweening;
using GameCore.GameScene.LevelItems.Character.Modules;
using JetBrains.Annotations;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Models;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Capacity;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using GameBasicsSDK.Modules.IdleArcade.Misc;
using GameBasicsSignals;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.UI.Products
{
    public class StackCounter : InjCoreMonoBehaviour
    {
        [SerializeField] private Transform _icon;
        [SerializeField] private TextMeshProUGUI _text;
        
        [Inject, UsedImplicitly] public SignalHub hub { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        public MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        public ProductsCarrier carrier =>  mainCharacterView.GetModule<StackModule>().carrier;

        private Color _textColorDefault;

        private Tween _colorTween;
        private Tween _scaleTween;

        public override void Construct()
        {
            initializeInOrderController.Add(Initialize, 10000);
            
            hub.Get<IASgnl.Misc.Max>().On(OnMax);
            _textColorDefault = _text.color;
        }

        private void Initialize()
        {
            carrier.onChange.On(UpdateAmount);
            UpdateAmount();

            mainCharacterView.GetModule<InteractorCapacityModule>().onChange.On(UpdateAmount);
        }

        private void UpdateAmount()
        {
            _text.text = carrier.count + "/" + carrier.capacity;
        }

        private void OnMax(Vector3 pos)
        {
            if ((_scaleTween != null && _scaleTween.IsActive()) || (_colorTween != null && _colorTween.IsActive()))
            {
                return;
            }
            
            _scaleTween = _icon.DOScale(Vector3.one * 1.1f, 0.1875f).SetLoops(4, LoopType.Yoyo).SetEase(Ease.Linear).SetLink(gameObject);
            _colorTween = _text.DOColor(Color.red, 0.1875f).SetLoops(4, LoopType.Yoyo).SetEase(Ease.Linear).SetLink(gameObject);
        }
    }
}