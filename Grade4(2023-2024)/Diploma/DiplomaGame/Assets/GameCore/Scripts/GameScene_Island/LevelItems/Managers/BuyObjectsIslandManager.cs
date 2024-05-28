using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene_Island.LevelItems.Items;
using JetBrains.Annotations;
using GameBasicsCore.Game.SaveProperties.Api;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Managers
{
    public class BuyObjectsIslandManager : BuyObjectsManager
    {
        [Inject, UsedImplicitly] public ShipCraft shipCraft { get; }
        
        [SerializeField] private BuyObject _shipBuyObject;
        
        [SerializeField] private BuyObject _firstObjectSecondFloor;
        private TheSaveProperty<bool> _shipBuyObjectCompletedSaveProperty;

        public override BuyObject currentBuyObject => _shipBuyObject.isInBuyMode ? _shipBuyObject : base.currentBuyObject;
        
        public override void Construct()
        {
            base.Construct();
            initializeInOrderController.Add(ValidateShipBuyObject, 3000);
            _shipBuyObject.onBuy.Once(() =>
            {
                _shipBuyObjectCompletedSaveProperty.value = true;
            });
        }

        protected override void GetSaves()
        {
            _activeBuyObjectIndexSaveProperty = new(CommStr.ActiveBuyObjectIndex_Island, linkToDispose: gameObject);
            _watchedCutsceneSaveProperty = new(CommStr.WatchedCutsceneArrive_Island);
            _shipBuyObjectCompletedSaveProperty = new(CommStr.ShipBuyObjectCompleted_Island, linkToDispose: gameObject); 
        }
        
        public void ValidateShipBuyObject()
        {
            if (!_shipBuyObjectCompletedSaveProperty.value)
            {
                if (shipCraft.IsFullShipComplete())
                {
                    _shipBuyObject.SetInBuyModeInternal(true);
                    onSetInBuyModeBuyObject.Dispatch(_shipBuyObject);
                }
                else
                {
                    _shipBuyObject.DeactivateInternal();
                    shipCraft.onFullShipComplete.Once(() =>
                    {
                        _shipBuyObject.SetInBuyModeInternal(true);
                        onSetInBuyModeBuyObject.Dispatch(_shipBuyObject);
                    });
                }
            }
            else
            {
                _shipBuyObject.ActivateInternal();
            }
        }
        
        protected override void Cheats()
        {
            base.Cheats();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Complete first floor"))
            {
                CompleteFirstFloor();
            }
            GUILayout.EndHorizontal();
        }

        private void CompleteFirstFloor()
        {
            _activeBuyObjectIndexSaveProperty.value = buyObjects.IndexOf(_firstObjectSecondFloor);
            sceneLoader.Restart();
        }
        
        protected override void CompleteScene()
        {
            _shipBuyObjectCompletedSaveProperty.value = true;
            shipCraft.CompleteShipCheat();
            _shipBuyObject.Buy();
            base.CompleteScene();
        }
    }
}