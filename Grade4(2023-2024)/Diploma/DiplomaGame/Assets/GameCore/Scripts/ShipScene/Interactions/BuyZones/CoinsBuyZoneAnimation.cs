using GameCore.Common.Sounds.Api;
using GameCore.ShipScene.Currency;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Models;
using GameBasicsCore.Game.Presenters;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Interactions
{
    public class CoinsBuyZoneAnimation : BuyZoneAnimation
    {
        [SerializeField] private int _maxBunches = 30;
        
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public ShipCoinsCurrencyPresenter shipCoinsCurrencyPresenter { get; }
        [Inject, UsedImplicitly] public BunchPopUpsMoveAnimationPresenter bunchPopUpsMoveAnimationPresenter { get; }
        [InjectOptional, UsedImplicitly] public ISoftCurrencyUISfxPlayer softCurrencyUISfxPlayer { get; }
        
        private const string BunchAreaId = "BuyZone";

        protected override void OnUsed(int useAmount)
        {
            int countBunches = useAmount;
            if (countBunches > _maxBunches)
                countBunches = _maxBunches;
            int toAdd = useAmount / countBunches;
            int addAtTheEnd = useAmount - (toAdd * countBunches);
            
            bunchPopUpsMoveAnimationPresenter.AddArea(buyZone.buyPopUp.scArea);
           
            var animationModel = bunchPopUpsMoveAnimationPresenter.Spawn(
                buyZone.buyPopUp.scArea.id,
                countBunches,
                shipCoinsCurrencyPresenter.collectedDisplay.GetIconPosition(),
                buyZone.buyPopUp.transform.position
            );

            animationModel.onChange.On(code =>
            {
                if (code == BunchPopUpsAnimationModel.ItemComplete)
                {
                    added.Dispatch(toAdd);
                    softCurrencyUISfxPlayer?.PlayCollected();
                }
                else if (code == BunchPopUpsAnimationModel.ItemSpawn)
                {
                    softCurrencyUISfxPlayer?.PlaySpawned();
                }
                else if (code == BunchPopUpsAnimationModel.Complete)
                {
                    added.Dispatch(addAtTheEnd);
                    completed.Dispatch();
                    bunchPopUpsMoveAnimationPresenter.RemoveArea(buyZone.buyPopUp.scArea);
                }
            });
        }
        
       
    }
}