using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Items;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Tutorials
{
    public class HitTreeTutorial : RaftTutorial
    {
        [SerializeField] private PalmRaftHittableItem _tree;
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }

        public override Vector3 targetPos => _tree.transform.position + Vector3.up * 1.5f;

        private TheSaveProperty<bool> _isTreeHittingTutorialPassed;
        
        public override void Construct()
        {
            base.Construct();
            
            _isTreeHittingTutorialPassed = new(CommStr.TreeHittingTutorialPassed);
           
            if (!_isTreeHittingTutorialPassed.value)
            {
                buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.On(TryStartTutorial);
                _tree.onTurnOff.Once(CompleteTutorial);
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
                if (_tree.gameObject.activeInHierarchy && !_isTreeHittingTutorialPassed.value)
                {
                    if (_tree.isEnabled)
                    {
                        StartTutorial();
                        buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.Off(TryStartTutorial);
                    }
                    else
                    {
                        _tree.onEndedRespawn.Once(() =>
                        {
                            if (!_isTreeHittingTutorialPassed.value)
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

            _isTreeHittingTutorialPassed.value = true;
        }
    }
}