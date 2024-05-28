using System.Collections;
using Cinemachine;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Animals.FightAnimal;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.DataConfigs;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class FirstWolfCutscene : Cutscene
    {
        [InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }
        [Inject, UsedImplicitly] public FightAnimalsManager fightAnimalsManager { get; }
        
        [SerializeField] private BuyObject _activateBuyObject;
        [SerializeField] private FightAnimalView _wolfView;
        [SerializeField] private CinemachineVirtualCamera _workbenchCam;
        [SerializeField] private CinemachineVirtualCamera _wolfCam;
        
        [SerializeField] private string _productName;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Sprite _iconOutline;
        [SerializeField] private ProductPriceDataConfig _priceDC;
        
        protected override bool deactivateMainCharacter => true;

        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 2000);
        }

        private void Initialize()
        {
            if (_activateBuyObject.isInBuyMode)
            {
                StartCutscene();
            }
            else
            {
                _activateBuyObject.onSetInBuyMode.Once(StartCutscene);
            }
        }

        protected override IEnumerator CutsceneCoroutine()
        {
            SwitchToCamera(_workbenchCam.gameObject);
            yield return new WaitForSeconds(3f);
            fightAnimalsManager.ActivateAnimal(_wolfView);
            SwitchToCamera(_wolfCam.gameObject);
            yield return new WaitForSeconds(3f);
            TurnOffCurrentCamera();
            yield return new WaitForSeconds(1.5f);
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
            }, false);
        }
    }
}