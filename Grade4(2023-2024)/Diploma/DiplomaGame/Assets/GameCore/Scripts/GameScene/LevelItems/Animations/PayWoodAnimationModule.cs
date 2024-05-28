using System;
using DG.Tweening;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products.Modules;
using GameBasicsSDK.Modules.IdleArcade.Settings;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Animations
{
    public class PayWoodAnimationModule : AddAnimationModule
    {
        [SerializeField] private float _jumpPower = 0.5f;
        
        public InteractSettings settings => InteractSettings.def;

        public override void Play(ProductView product, Vector3 endPosition, Vector3 endAngles, Action onComplete)
        {
            product.transform.DOScale(0f, settings.addingProductDuration)
                .SetEase(Ease.InBack);
            
            product.transform.DOLocalJump(endPosition, _jumpPower, 1, settings.addingProductDuration)
                .SetId(product)
                .OnComplete(onComplete.Invoke).SetLink(gameObject);
        }
    }
}