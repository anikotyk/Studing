using System.Collections.Generic;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class CuttableZone : InjCoreMonoBehaviour
    {
        [SerializeField] private ProductDataConfig _productDataConfig;
        private List<InteractorCharacterModel> _interactors = new List<InteractorCharacterModel>();
        public void OnEnterZone(InteractorCharacterModel interactorModel)
        {
            if (_interactors.Contains(interactorModel)) return;
            
            var cuttingModule = interactorModel.view.GetModule<CuttingModule>();
            if (cuttingModule == null || !cuttingModule.IsAbleToCut(_productDataConfig)) return;
            if (!cuttingModule.CanInteract()) return;
            
            if (!_interactors.Contains(interactorModel))
            {
                _interactors.Add(interactorModel);
            }
            
            cuttingModule.StartCutting();
        }
        
        public void OnExitZone(InteractorCharacterModel interactorModel)
        {
            var cuttingModule = interactorModel.view.GetModule<CuttingModule>();
            if (cuttingModule == null || !cuttingModule.IsAbleToCut(_productDataConfig)) return;
            
            if (!_interactors.Contains(interactorModel)) return;
            _interactors.Remove(interactorModel);
            
            cuttingModule.EndCutting();
        }
    }
}