using Dreamteck.Splines;
using GameCore.Common.LevelItems;
using GameCore.GameScene_Iceland.LevelItems.Animals.KillerWhale.Modules;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using UnityEngine;

namespace GameCore.GameScene_Iceland.LevelItems.Animals.KillerWhale
{
    public class WhaleKillerView : CharacterView
    {
        private HealthModule _healthAnimalModuleCached;
        public HealthModule healthAnimalModule => _healthAnimalModuleCached ??=
            GetComponentInChildren<HealthModule>(true);
        
        private WhaleKillerGetDamageModule _getDamageModuleCached;
        public WhaleKillerGetDamageModule getDamageModule => _getDamageModuleCached ??=
            GetComponentInChildren<WhaleKillerGetDamageModule>(true);
        
        private WhaleKillerWalkBySplineModule _walkBySplineModule;
        public WhaleKillerWalkBySplineModule walkBySplineModule => _walkBySplineModule ??=
            GetComponentInChildren<WhaleKillerWalkBySplineModule>(true);
        
        private WhaleKillerAttackingModule _attackingModuleCached;
        public WhaleKillerAttackingModule attackingModule => _attackingModuleCached ??=
            GetComponentInChildren<WhaleKillerAttackingModule>(true);

        private SplineFollower _splineFollowerCached;
        public SplineFollower splineFollower => _splineFollowerCached ??=
            GetComponent<SplineFollower>();

        public void ResetView()
        {
            SetIdle();
            attackingModule.StopAttack();
            attackingModule.UnlockStartAttack();
            healthAnimalModule.ResetHealth(false);
            walkBySplineModule.StartWalk();
            gameObject.SetActive(true);
            animator.gameObject.SetActive(true);
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            animator.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}