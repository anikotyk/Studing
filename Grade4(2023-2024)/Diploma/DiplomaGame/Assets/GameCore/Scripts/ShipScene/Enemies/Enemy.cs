using System.Collections.Generic;
using GameCore.ShipScene.Enemies.Combat;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using GameBasicsSignals;
using Pathfinding;
using StaserSDK.Animations;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Enemies
{
    public class Enemy : InjCoreMonoBehaviour, IPoolItem<Enemy>
    {
        [SerializeField] private string _id;
        [SerializeField] private Health _health;
        [SerializeField] private AIPath _aiPath;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private EnemyAttackModule _enemyAttackModule;
        [SerializeField] private AnimatorLinker _animatorLinker;

        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        public MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        
        public EnemyAttackModule attackModule => _enemyAttackModule;
        public Health health => _health;
        public AIPath aiPath => _aiPath;
        public Rigidbody rigidbody => _rigidbody;
        public string id => _id;
        public Animator animator => _animatorLinker.animator;

        public bool isInjected { get; private set; } = false;
        
        private List<object> _seekerBlockers = new();
        
        public IPool<Enemy> pool { get; set; }
        public bool isTook { get; set; }
        
        public TheSignal injected { get; } = new();
        public TheSignal<Enemy> died { get; } = new();
        
        public override void Construct()
        {
            isInjected = true;
            injected.Dispatch();
            _health.died.On(() => died.Dispatch(this));
        }
        
        public void DisableMovement(object sender)
        {
            if(_seekerBlockers.Contains(sender) == false)
                _seekerBlockers.Add(sender);
            aiPath.enableRotation = false;
            aiPath.canMove = false;
        }

        public void EnableMovement(object sender)
        {
            _seekerBlockers.Remove(sender);
            if(_seekerBlockers.Count > 0)
                return;

            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            aiPath.enableRotation = true;
            aiPath.canMove = true;
        }

        public void TakeItem()
        {
        }

        public void ReturnItem()
        {
            _seekerBlockers.Clear();
            EnableMovement(this);
            health.Revive();
        }
    }
}