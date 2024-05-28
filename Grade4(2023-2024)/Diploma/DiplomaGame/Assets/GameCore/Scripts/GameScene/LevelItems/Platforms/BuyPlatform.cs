using GameCore.Common.LevelItems;
using GameCore.GameScene.Saves;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Platforms
{
    public class BuyPlatform : DonateResourcesPlatform
    {
        [Inject, UsedImplicitly] public override PaidWoodSaveProperty paidWoodSaveProperty { get; }
        [Inject, UsedImplicitly] public override PaidProductsSaveData paidProductsSaveData { get; }
        [Inject, UsedImplicitly] public override PaidSCSaveProperty paidScSaveProperty { get; }

        
        [SerializeField] private DonateBuyObject _buyingObject;

        public override void DonateComplete()
        {
            base.DonateComplete();
            _buyingObject.Buy();
        }
    }
}