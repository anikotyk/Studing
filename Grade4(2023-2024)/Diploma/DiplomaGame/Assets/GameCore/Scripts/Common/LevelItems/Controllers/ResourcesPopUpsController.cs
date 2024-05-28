using System.Collections.Generic;
using GameCore.Common.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using Zenject;

namespace GameCore.Common.LevelItems.Controllers
{
    public class ResourcesPopUpsController : ControllerInternal
    {
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }

        private List<ResourcePopUp> _getResourcePopUps = new List<ResourcePopUp>();
        public List<ResourcePopUp> deactivatedGetResourcePopUps => _getResourcePopUps.FindAll((item)=>!item.gameObject.activeSelf);
        private List<ResourcePopUp> _loseResourcePopUps = new List<ResourcePopUp>();
        public List<ResourcePopUp> deactivatedLoseResourcePopUps => _loseResourcePopUps.FindAll((item)=>!item.gameObject.activeSelf);

        public void SpawnPopUpGetResource(ProductView product)
        {
            ResourcePopUp popUp;
            if (deactivatedGetResourcePopUps.Count <= 0)
            {
                popUp = popUpsController.SpawnUnderMenu<ResourcePopUp>("GetResourcePopUp");
                popUp.worldSpaceConverter.updateMethod = UpdateMethod.None;
            }
            else
            {
                popUp = deactivatedGetResourcePopUps[0];
            } 
            popUp.worldSpaceConverter.followWorldObject = product.transform;
            popUp.SetSprite(product.dataConfig.icon);
            popUp.gameObject.SetActive(true);
        }
        
        public void SpawnPopUpLoseResource(ProductView product)
        {
            ResourcePopUp popUp;
            if (deactivatedLoseResourcePopUps.Count <= 0)
            {
                popUp = popUpsController.SpawnUnderMenu<ResourcePopUp>("LoseResourcePopUp");
                popUp.worldSpaceConverter.updateMethod = UpdateMethod.None;
            }
            else
            {
                popUp = deactivatedLoseResourcePopUps[0];
            } 
            popUp.worldSpaceConverter.followWorldObject = product.transform;
            popUp.SetSprite(product.dataConfig.icon);
            popUp.gameObject.SetActive(true);
        }
    }
}