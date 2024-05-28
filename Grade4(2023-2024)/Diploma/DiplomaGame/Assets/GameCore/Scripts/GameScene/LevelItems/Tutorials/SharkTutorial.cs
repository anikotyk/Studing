using DG.Tweening;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Managers;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems.Tutorials
{
    public class SharkTutorial : RaftTutorial
    {
        [SerializeField] private MeatAnimalSplinableItem meatAnimalItem;
        [Inject, UsedImplicitly] public StallObject stallObject { get; }
        [InjectOptional, UsedImplicitly] public StallCutsceneManager stallCutsceneManager { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        public override Vector3 targetPos => meatAnimalItem.transform.position + Vector3.up * 1f;

        private TheSaveProperty<bool> _isSharkTutorialPassed;
        public TheSaveProperty<bool> isSharkTutorialPassed => _isSharkTutorialPassed;
        
        public override void Construct()
        {
            base.Construct();
            
            _isSharkTutorialPassed = new(CommStr.SharkTutorialPassed);
           
            if (!_isSharkTutorialPassed.value)
            {
                initializeInOrderController.Add(Initialize, 1000);
            }
        }

        private void Initialize()
        {
            meatAnimalItem.onDead.Once(CompleteTutorial);
            
            if (!stallObject.gameObject.activeInHierarchy)
            {
                if (stallCutsceneManager)
                {
                    stallCutsceneManager.onCutsceneEnded.Once(TryStartTutorial);
                }
                else
                {
                    stallObject.buyObject.onBuy.Once(TryStartTutorial);
                }
            }
            else
            {
                TryStartTutorial();
            }
        }

        private void TryStartTutorial()
        {
            DOVirtual.DelayedCall(0.05f, () =>
            {
                if (stallObject.gameObject.activeInHierarchy && !_isSharkTutorialPassed.value)
                {
                    StartTutorial();
                }
            },false).SetLink(gameObject);
        }
        
        public override void StartTutorial()
        {
            base.StartTutorial();
            
            meatAnimalItem.gameObject.SetActive(true);
            meatAnimalItem.Activate();
            meatAnimalItem.splineFollower.onEndReached += (_) =>
            {
                if (!meatAnimalItem.isDead)
                {
                    meatAnimalItem.Activate();
                }
            };
        }

        public override void CompleteTutorial()
        {
            base.CompleteTutorial();

            _isSharkTutorialPassed.value = true;
            meatAnimalItem.gameObject.SetActive(false);
        }
    }
}