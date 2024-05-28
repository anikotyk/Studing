using System.Collections;
using Cinemachine;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.DataConfigs;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class FirstBearCutscene : Cutscene
    {
        [InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }
        
        [SerializeField] private BuyObject _activateBuyObject;
        [SerializeField] private BuyObject _workbenchBuyObject;
        [SerializeField] private CinemachineVirtualCamera _workbenchCam;
        [SerializeField] private CinemachineVirtualCamera _bearCam;
        
        [SerializeField] private string _productName;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Sprite _iconOutline;
        [SerializeField] private ProductPriceDataConfig _priceDC;
        
        protected override bool deactivateMainCharacter => false;

        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 2000);
        }

        private void Initialize()
        {
            if (_activateBuyObject.isBought && !_workbenchBuyObject.isBought)
            {
                StartCutscene();
            }
            else
            {
                _activateBuyObject.onBuy.Once(StartCutscene);
            }
        }

        protected override IEnumerator CutsceneCoroutine()
        {
            SwitchToCamera(_workbenchCam.gameObject);
            yield return new WaitForSeconds(1f);
            _workbenchBuyObject.Buy();
            yield return new WaitForSeconds(2f);
            SwitchToCamera(_bearCam.gameObject);
            yield return new WaitForSeconds(3f);
            TurnOffCurrentCamera();
            yield return new WaitForSeconds(0.5f);
            OnEndScene();
            ShowProductWindow();
        }
        
        private void ShowProductWindow()
        {
            hapticService?.Selection();
            
            windowFactory.Create<UnlockNewProductDialog>("UnlockNewProductDialog", window =>
            {
                window.Initialize(_productName, _priceDC.softCurrencyCount, _icon, _iconOutline);
                window.Show();
                window.onShowComplete.Once((_) =>
                {
                    mainCharacterView.gameObject.SetActive(false);
                });
                window.onHideComplete.Once((_) =>
                {
                    mainCharacterView.gameObject.SetActive(true);
                });
            }, false);
        }
    }
}