using GameCore.Common.Misc;
using GameCore.Common.Sounds.Api;
using GameCore.GameScene.Settings;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Factories;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.InteractItems
{
    public class UpgradesPlatformInteractItem : InteractItem
    {
        [SerializeField] private string _dialogWindowId;
        [SerializeField] private bool _pauseWhenDialogShows;
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        [Inject, UsedImplicitly] public VfxStack vfxStack { get; }
        [InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }
        [InjectOptional, UsedImplicitly] public IUpgradeUnitSfxPlayer upgradeUnitSfxPlayer { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        [Inject, UsedImplicitly] public UpgradesController upgradesController { get; }
        public MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        public override int priority { get; } = 2;

        private bool _isRunning;
        private bool _isUpgraded;
        
        public TheSignal showed { get; } = new();
        public TheSignal hided { get; } = new();
        
        public override bool CanInteract(InteractorCharacterModel interactorModel)
        {
            return interactorModel.view is MainCharacterView;
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (_isRunning) return;
            
            hapticService?.Selection();
            showed.Dispatch();
            _isUpgraded = false;
            
            windowFactory.Create<UpgradesDialog>(_dialogWindowId, window =>
            {
                _isRunning = true;
                window.Show();

                window.onHideStart.Once(_ =>
                {
                    hided.Dispatch();
                    _isRunning = false;
                });
                
                GameplaySettings.def.upgradesGroup.ForEach((config) =>
                {
                    var model = upgradesController.GetModel(config);
                    model.onChange.Once((_)=>
                    {
                        _isUpgraded = true;
                    }).OffWhen(() => !_isRunning);
                });
                
                window.onHideComplete.Once((_) =>
                {
                    if (_isUpgraded)
                    {
                        ShowUpgradeEffects();
                    }
                });

            }, _pauseWhenDialogShows);
        }
        
        private void ShowUpgradeEffects()
        {
            var vfx = vfxStack.Spawn(CommStr.UpgradeVerticalGlowVFX, mainCharacterView.transform.position);
            vfx.transform.SetParent(mainCharacterView.transform);
            upgradeUnitSfxPlayer?.Play();
        }
    }
}