using GameCore.GameScene_Iceland.Saves;
using GameCore.GameScene.LevelItems.Platforms;
using GameCore.GameScene.Saves;
using JetBrains.Annotations;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Items
{
    public class StartHuntingDonatePlatform : DonateResourcesPlatform
    {
        [Inject, UsedImplicitly] public PaidWoodForBoatHuntingSaveProperty paidWoodForBoatHuntingSaveProperty { get; }
        [Inject, UsedImplicitly] public PaidProductsForBoatHuntingSaveData paidProductsForBoatHuntingSaveData { get; }
        [Inject, UsedImplicitly] public PaidSCForBoatHuntingSaveProperty paidScForBoatHuntingSaveProperty { get; }

        public override PaidWoodSaveProperty paidWoodSaveProperty => paidWoodForBoatHuntingSaveProperty;
        public override PaidProductsSaveData paidProductsSaveData  => paidProductsForBoatHuntingSaveData;
        public override PaidSCSaveProperty paidScSaveProperty  => paidScForBoatHuntingSaveProperty;
        
        protected override bool canCompleteDonationOnValidate => false;
    }
}