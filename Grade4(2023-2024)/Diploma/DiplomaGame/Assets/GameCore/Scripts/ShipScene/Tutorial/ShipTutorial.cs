using System.Collections.Generic;
using GameCore.ShipScene.Battle;
using GameCore.ShipScene.Battle.Waves;
using GameCore.ShipScene.Common;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.SaveProperties.Api;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Tutorial
{
    public class ShipTutorial : InjCoreMonoBehaviour
    {
        [SerializeField] private List<GameObject> _objectsToHide;
        [SerializeField] private PointingHand _pointingHand;

        [Inject, UsedImplicitly] public BattleController battleController { get; }

        private TheSaveProperty<bool> _tutorialFinishedCached;
        private TheSaveProperty<bool> tutorialFinishedProperty =>
            _tutorialFinishedCached ??=
                new TheSaveProperty<bool>("TutorialFinished", domain: ShipSceneConstants.saveFile);

        public override void Construct()
        {
            if (tutorialFinishedProperty.value)
            {
                EndTutorial();
                return;
            }

            battleController.started.Once(OnBattleStarted);
            StartTutorial();
        }

        private void OnBattleStarted(Wave wave)
        {
            tutorialFinishedProperty.value = true;
            EndTutorial();
        }

        private void StartTutorial()
        {
            _pointingHand.PlayTaps(-1);
            _pointingHand.GetComponent<CanvasGroup>().alpha = 1.0f;
            _pointingHand.gameObject.SetActive(true);
            _objectsToHide.ForEach(x=>x.SetActive(false));
        }

        private void EndTutorial()
        {
            _pointingHand.Stop();
            _pointingHand.gameObject.SetActive(false);
            _objectsToHide.ForEach(x=>x.SetActive(true));
        }
    }
}