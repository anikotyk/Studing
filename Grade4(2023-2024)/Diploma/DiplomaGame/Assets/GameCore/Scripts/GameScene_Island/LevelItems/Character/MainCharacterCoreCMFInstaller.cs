using CMF;
using GameBasicsCore.Tools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.Controllers.CharacterContext.MainCharacter;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Character
{
    public class MainCharacterCoreCMFInstaller : MonoInstaller
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private MainCharacterSimpleWalkerController _simpleWalkerController;
        [SerializeField] private ScreenJoystickCharacterInput _screenJoystickCharacterInput;

        public override void InstallBindings()
        {
            Container.BindInstance(_mover);
            Container.BindInstance(_simpleWalkerController);
            Container.BindInstance(_screenJoystickCharacterInput);

            Container.BindDefault<JoystickCharacterInputController>();
            Container.BindDefault<MainCharacterLocomotionRatioCoreController>();
            Container.BindDefault<MainCharacterWalkerCoreSpeedController>();  // Can be moved to own installer
        }
    }
}