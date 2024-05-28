using System.Collections;
using Cinemachine;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Factories;
using GameBasicsCore.Game.Views.UI.Blockers;
using GameBasicsCore.Game.Views.UI.Windows.Menus;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Managers
{
    public abstract class Cutscene : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public GameplayUIMenuWindow gameplayUIMenuWindow { get; }
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public MenuBlockOverlay menuBlockOverlay { get; }
        [Inject, UsedImplicitly] public TopTextHint topTextHint { get; }
        [Inject, UsedImplicitly] public TutorialArrow3D tutorialArrow3D { get; }
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }

        protected MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;

        private GameObject _currentCam;

        protected virtual bool deactivateGameplayUIWindow => true;
        protected virtual bool deactivateTutorialArrow => true;
        protected virtual bool activateMenuBlockOverlay => true;
        protected virtual bool deactivateMainCharacter => true;
        
        public readonly TheSignal onEndedScene = new();

        public virtual void StartCutscene()
        {
            OnStartScene();
            StartCoroutine(CutsceneCoroutine());
        }

        protected virtual IEnumerator CutsceneCoroutine()
        {
            yield return null;
        }

        protected virtual void OnStartScene()
        {
            if (deactivateGameplayUIWindow)
            {
                gameplayUIMenuWindow.Hide();
            }
            if (deactivateTutorialArrow)
            {
                tutorialArrow3D.Disable();
            }
            if (activateMenuBlockOverlay)
            {
                menuBlockOverlay.Activate(this);
            }
            if (deactivateMainCharacter)
            {
                mainCharacterView.gameObject.SetActive(false);
            }
            
        }
        
        protected virtual void OnEndScene()
        {
            if (deactivateGameplayUIWindow)
            {
                gameplayUIMenuWindow.Show();
            }
            if (deactivateTutorialArrow)
            {
                tutorialArrow3D.Enable();
            }
            if (activateMenuBlockOverlay)
            {
                menuBlockOverlay.Deactivate(this);
            }
            if (deactivateMainCharacter)
            {
                mainCharacterView.gameObject.SetActive(true);
            }
            
            onEndedScene.Dispatch();
        }
        
        protected void SwitchToCamera(GameObject cam)
        {
            TurnOffCurrentCamera();
            cam.gameObject.SetActive(true);
            _currentCam = cam;
        }
        
        protected void SwitchToCamera(CinemachineVirtualCamera cam)
        {
            SwitchToCamera(cam.gameObject);
        }

        protected void TurnOffCurrentCamera()
        {
            if (_currentCam != null)
            {
                _currentCam.gameObject.SetActive(false);
            }
        }
    }
}