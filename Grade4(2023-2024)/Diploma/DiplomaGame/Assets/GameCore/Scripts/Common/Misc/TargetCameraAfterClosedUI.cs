using GameCore.Common.Controllers;
using GameCore.Common.LevelItems;
using GameCore.GameScene.LevelItems.Platforms;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Misc
{
    public class TargetCameraAfterClosedUI : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public TargetCameraOnObjectController targetCameraOnObjectController { get; }
        [SerializeField] private BuyObject _prevObject;

        public override void Construct()
        {
            base.Construct();
            _prevObject.GetComponent<IWindowShowable>().onWindowClosed.Once(() =>
            {
                Show();
            });
        }

        private void Show()
        {
            var target = GetComponentInChildren<DonateResourcesPlatform>(true);
            if (!target) return;
            targetCameraOnObjectController.ShowObject(target.transform);
        }
    }
}