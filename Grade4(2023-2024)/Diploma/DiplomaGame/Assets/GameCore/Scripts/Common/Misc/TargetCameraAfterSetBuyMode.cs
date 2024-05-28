using GameCore.Common.Controllers;
using GameCore.GameScene.LevelItems.Platforms;
using JetBrains.Annotations;
using Zenject;

namespace GameCore.Common.Misc
{
    public class TargetCameraAfterSetBuyMode : ActionAfterSetBuyMode
    {
        [Inject, UsedImplicitly] public TargetCameraOnObjectController targetCameraOnObjectController { get; }
        
        protected override void Action()
        {
            var target = GetComponentInChildren<DonateResourcesPlatform>(true);
            if (!target) return;
            targetCameraOnObjectController.ShowObject(target.transform);
        }
    }
}