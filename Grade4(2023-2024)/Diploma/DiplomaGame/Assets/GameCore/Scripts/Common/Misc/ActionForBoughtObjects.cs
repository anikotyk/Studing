using GameCore.Common.LevelItems;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using Zenject;

namespace GameCore.Common.Misc
{
    public class ActionForBoughtObjects : ActionAfterBuy
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        public override void Construct()
        {
            base.Construct();
            initializeInOrderController.Add(Initialize, 2000);
        }

        private void Initialize()
        {
            if (buyObject.isBought)
            {
                Action();
            }
        }
    }
}