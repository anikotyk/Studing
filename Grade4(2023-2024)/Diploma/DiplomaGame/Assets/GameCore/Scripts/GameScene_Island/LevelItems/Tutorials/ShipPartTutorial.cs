using DG.Tweening;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.LevelItems.Tutorials;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Tutorials
{
    public class ShipPartTutorial : RaftTutorial
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        
        [SerializeField] private ShipPart _shipPart;
        [SerializeField] private float _delay = 2f;
        public override Vector3 targetPos => _shipPart.transform.position + Vector3.up * 0.5f;

        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 20000);
        }

        private void Initialize()
        {
            _shipPart.onTaken.Once(CompleteTutorial);
            
            if (_shipPart.isActive)
            {
                StartTutorial();
            }
            else
            {
                _shipPart.onActivate.Once(()=>
                {
                    DOVirtual.DelayedCall(_delay, StartTutorial, false);
                });
            }
        }
    }
}