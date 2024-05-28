using DG.Tweening;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.LevelItems.Tutorials;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Tutorials
{
    public class HogTrapTutorial : RaftTutorial
    {
        [SerializeField] private AnimalTrap _trap;
        public override Vector3 targetPos => _trap.sellProductsCollectPlatform.transform.position + Vector3.up * 0.5f;

        public override void Construct()
        {
            base.Construct();

            _trap.sellProductsCollectPlatform.onFull.Once(CompleteTutorial);
        }
    }
}