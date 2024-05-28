using GameCore.Common.LevelItems;
using GameCore.Common.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Factories;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Misc
{
    public class EventObtainObject : InjCoreMonoBehaviour, IWindowShowable
    {
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        
        [SerializeField] private string _objectName;
        [SerializeField] private string _objectDescription;
        [SerializeField] private Sprite _objectSprite;
        private BuyObject _buyObject;
        public TheSignal onWindowClosed => onClosedObtainDialog;
        public TheSignal onClosedObtainDialog { get; } = new();

        public override void Construct()
        {
            base.Construct();
            _buyObject = GetComponent<BuyObject>();
            _buyObject.onBuy.Once(() =>
            {
                windowFactory.Create<ObtainObjectDialog>(CommStr.ObtainObjectDialog,window =>
                {
                    window.SetObject(_objectSprite, _objectName, _objectDescription);
                    window.onHideComplete.Once((_) =>
                    {
                        onClosedObtainDialog.Dispatch();
                    });
                
                    window.Show();
                });
            });
        }
    }
}