using CMF;
using GameCore.GameScene_Iceland.Controllers;
using GameCore.GameScene_Iceland.LevelItems.Items;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Presenters;
using GameBasicsCore.Game.Settings.GameCore;
using GameBasicsCore.Tools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.Installers.CharacterContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using UnityEngine;

namespace GameCore.GameScene_Iceland.Installers
{
    public class BoatHuntingInstaller : InteractorCharacterInstaller<BoatHuntingView>
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private MainCharacterSimpleWalkerController _simpleWalkerController;
        [SerializeField] private ScreenJoystickCharacterInput _screenJoystickCharacterInput;
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.BindInstance(_mover);
            Container.BindInstance(_simpleWalkerController);
            Container.BindInstance(_screenJoystickCharacterInput);
            
            Container.BindDefault<BoatHuntingLocomotionRatioController>();

            if (GameCoreSettings.def.controls.enableJoystick)
            {
                Container.BindDefault<JoystickController>();
                Container.BindDefault<JoystickPresenter>();
            }

            Container.BindDefault<JoystickBoatInputController>();
        }
    }
}