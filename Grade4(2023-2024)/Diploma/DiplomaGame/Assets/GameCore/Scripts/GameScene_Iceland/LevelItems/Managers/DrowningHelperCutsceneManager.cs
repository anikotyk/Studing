using System.Collections;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.PopUps;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class DrowningHelperCutsceneManager : Cutscene
    {
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [SerializeField] private BuyObject _activateHelperInWaterBuyObject;
        [SerializeField] private BuyObject _deactivateHelperInWaterBuyObject;
        [SerializeField] private GameObject _helperInWater;
        [SerializeField] private GameObject _helperInWaterCam;
        [SerializeField] private Transform _popUpPoint;
        [SerializeField] private AnimatorParameterApplier _helpAnim;
        [SerializeField] private AnimatorParameterApplier _swimAnim;
        
        private TextPopUpView _helpPopUp;
        protected override bool deactivateMainCharacter => false;

        public override void Construct()
        {
            base.Construct();
            _activateHelperInWaterBuyObject.onBuy.Once(StartCutscene);
            _deactivateHelperInWaterBuyObject.onBuy.Once(DeactivateHelperInWater);

            initializeInOrderController.Add(Initialize, 2000);
        }
        
        
        private void Initialize()
        {
            if (_activateHelperInWaterBuyObject.isBought && !_deactivateHelperInWaterBuyObject.isBought)
            {
                ActivateHelperInWater();
            }
        }

        private void ActivateHelperInWater()
        {
            _helperInWater.SetActive(true);
            _swimAnim.Apply();
            ShowHelpPopUp();
        }
        
        private void DeactivateHelperInWater()
        {
            _helperInWater.SetActive(false);
            if(_helpPopUp) _helpPopUp.gameObject.SetActive(false);
        }

        private void ShowHelpPopUp()
        { 
            _helpPopUp = popUpsController.SpawnUnderMenu<TextPopUpView>("Help");
            _helpPopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
            _helpPopUp.worldSpaceConverter.followWorldObject = _popUpPoint;
            _helpPopUp.transform.localScale = Vector3.one;
        }
        
        protected override IEnumerator CutsceneCoroutine()
        {
            popUpsController.containerUnderMenu.gameObject.SetActive(false);
            popUpsController.containerOverMenu.gameObject.SetActive(false);
            ActivateHelperInWater();
            yield return new WaitForSeconds(0.5f);
            SwitchToCamera(_helperInWaterCam);
            yield return new WaitForSeconds(2f);
            _helpAnim.Apply();
            topTextHint.ShowHint("Poor thing, in cold water");
            yield return new WaitForSeconds(2f);
            TurnOffCurrentCamera();
            yield return new WaitForSeconds(2f);
            popUpsController.containerUnderMenu.gameObject.SetActive(true);
            popUpsController.containerOverMenu.gameObject.SetActive(true);
            buyObjectsManager.ShowCurrentBuyObject();
            OnEndScene();
        }
    }
}