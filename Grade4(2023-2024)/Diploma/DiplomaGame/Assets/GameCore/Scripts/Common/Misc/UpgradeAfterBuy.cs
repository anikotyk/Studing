using JetBrains.Annotations;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Models;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Misc
{
    public class UpgradeAfterBuy : ActionAfterBuy
    {
        [Inject, UsedImplicitly] public UpgradesController upgradesController { get; }
        
        [SerializeField] private UpgradePropertyDataConfig _config;
        [SerializeField] private int _cntStages;
        
        private UpgradePropertyModel _model;

        protected override void Action()
        {
            base.Action();
            Upgrade(); 
        }

        private void Upgrade()
        {
            _model = upgradesController.GetModel(_config);
            for (int i = 0; i < _cntStages; i++)
            {
                _model.TryUpgrade();
            }
        }
    }
}