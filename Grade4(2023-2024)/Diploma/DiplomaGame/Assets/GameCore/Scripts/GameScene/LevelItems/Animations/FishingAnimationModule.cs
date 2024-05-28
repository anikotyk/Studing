using System;
using DG.Tweening;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products.Modules;
using GameBasicsSDK.Modules.IdleArcade.Settings;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Animations
{
    public class FishingAnimationModule : AddAnimationModule
    {
        [SerializeField] private float _jumpPower = 0.5f;
        
        public InteractSettings settings => InteractSettings.def;

        public override void Play(ProductView product, Vector3 endPosition, Vector3 endAngles, Action onComplete)
        {
            product.transform.localScale = Vector3.one * 0.5f;
            
            product.transform.DOScale(1f, settings.addingProductDuration/4f).SetEase(Ease.InOutBack);
            
            product.transform.DOLocalJump(endPosition, _jumpPower, 1, settings.addingProductDuration)
                .SetId(product)
                .OnComplete(onComplete.Invoke).SetLink(gameObject);
        }
    }
}