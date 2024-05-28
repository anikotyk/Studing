using DG.Tweening;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.LevelItems.Tutorials;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Tutorials
{
    public class RecipeTutorial : RaftTutorial
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        
        [SerializeField] private Recipe _recipe;
        [SerializeField] private float _delay = 2f;
        public override Vector3 targetPos => _recipe.transform.position + Vector3.up * 0.5f;

        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 20000);
        }

        private void Initialize()
        {
            _recipe.onTaken.Once(CompleteTutorial);
            
            if (_recipe.isActive)
            {
                StartTutorial();
            }
            else
            {
                _recipe.onActivate.Once(()=>
                {
                    DOVirtual.DelayedCall(_delay, StartTutorial, false);
                });
            }
        }
    }
}