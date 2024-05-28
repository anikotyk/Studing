using GameCore.GameScene.Controllers.ObjectContext;
using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using Zenject;

namespace GameCore.GameScene.LevelItems.Platforms
{
    public class SellPlatform : InteractPlatform
    {
        [Inject, UsedImplicitly] public SellCollectingController sellCollectingController { get; }
        private InteractItem _interactItemCached;
        public InteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<InteractItem>();
                return _interactItemCached;
            }
        }

    }
}