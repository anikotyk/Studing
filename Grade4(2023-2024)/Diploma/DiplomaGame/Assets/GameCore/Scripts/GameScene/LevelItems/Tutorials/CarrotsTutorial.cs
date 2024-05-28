using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene_Island.LevelItems.Items;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Tutorials
{
    public class CarrotsTutorial : RaftTutorial
    {
        [SerializeField] private CuttableItem _carrots;
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }

        public override Vector3 targetPos => _carrots.transform.position + Vector3.up * 0.5f;

        private TheSaveProperty<bool> _isCarrotsTutorialPassed;
        
        public override void Construct()
        {
            base.Construct();
            
            _isCarrotsTutorialPassed = new(CommStr.CarrotsTutorialPassed);
           
            if (!_isCarrotsTutorialPassed.value)
            {
                buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.On(TryStartTutorial);
                _carrots.onNotEnabled.Once(CompleteTutorial);
                initializeInOrderController.Add(Initialize, 1000);
            }
        }

        private void Initialize()
        {
            TryStartTutorial(0);
        }

        private void TryStartTutorial(int index)
        {
            DOVirtual.DelayedCall(0.05f, () =>
            {
                if (_carrots.gameObject.activeInHierarchy && !_isCarrotsTutorialPassed.value)
                {
                    if (_carrots.interactItem.enabled)
                    {
                        StartTutorial();
                        buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.Off(TryStartTutorial);
                    }
                    else
                    {
                        _carrots.onEnabled.Once(() =>
                        {
                            if (!_isCarrotsTutorialPassed.value)
                            {
                                StartTutorial();
                                buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.Off(TryStartTutorial);
                            }
                        });
                    }
                }
            },false).SetLink(gameObject);
        }

        public override void CompleteTutorial()
        {
            base.CompleteTutorial();

            _isCarrotsTutorialPassed.value = true;
        }
    }
}