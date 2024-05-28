using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Platforms;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Controllers;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Controllers
{
    public class BuyObjectTaskController : ControllerInternal
    {
        [InjectOptional, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [Inject, UsedImplicitly] public TaskPanelController taskPanelController { get; }
        [InjectOptional, UsedImplicitly] public IButtonUiClickSfxPlayer buttonUiClickSfxPlayer { get; }
        private TaskPanelModel _currentModel;

        public override void Construct()
        {
            base.Construct();
            
            if(!buyObjectsManager) return;
            buyObjectsManager.onSetInBuyModeBuyObject.On(ShowTask);
        }
        
        private void ShowTask(BuyObject buyObject)
        {
            if(_currentModel != null && !_currentModel.isDone) _currentModel.Done();
            if(buyObject.GetComponent<IgnoreTaskPanelItem>()) return;
            _currentModel = new TaskPanelModel();

            var platform = buyObject.GetComponentInChildren<DonateResourcesPlatform>(true);
            if (platform)
            {
                _currentModel.icon = platform.iconItem;  
            }
            _currentModel.description = buyObject.actionName +" "+ buyObject.objectName;
            _currentModel.amountTotalCallback = () => 1;
            _currentModel.getProgressTextCallback = () => $"{_currentModel.amountDone}/{1}";
            _currentModel.clickCallback = OnTaskPanelClick;

            if (!taskPanelController.lastView || taskPanelController.lastView.isDoneComplete)
            {
                taskPanelController.AddTask(_currentModel);
            }
            else
            {
                taskPanelController.lastView.onDoneComplete.Once(() =>
                {
                    taskPanelController.AddTask(_currentModel);
                });
            }
            buyObject.onBuy.Once(_currentModel.Done);
        }
        
        private void OnTaskPanelClick()
        {
            buyObjectsManager.ShowCurrentBuyObject();
            buttonUiClickSfxPlayer?.Play();
        }
    }
}