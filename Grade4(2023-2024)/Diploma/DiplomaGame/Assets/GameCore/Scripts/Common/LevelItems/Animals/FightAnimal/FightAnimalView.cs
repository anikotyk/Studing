using GameCore.Common.LevelItems.Animals.FightAnimal.Modules;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.Common.LevelItems.Animals.FightAnimal
{
    public class FightAnimalView : AnimalView
    {
        [SerializeField] private Transform[] _helperPoints;
        [SerializeField] private Sprite _icon;
        public Sprite icon => _icon;
        public Transform[] helperPoints => _helperPoints;
        private AnimalWalkRandomModule _walkRandomModuleCached;
        public AnimalWalkRandomModule walkRandomModule => _walkRandomModuleCached ??=
            GetComponentInChildren<AnimalWalkRandomModule>(true);
        
        private HealthModule _healthAnimalModuleCached;
        public HealthModule healthAnimalModule => _healthAnimalModuleCached ??=
            GetComponentInChildren<HealthModule>(true);
        
        private FightAnimalAttackModule _fightAnimalAttackModuleCached;
        public FightAnimalAttackModule fightAnimalAttackModule => _fightAnimalAttackModuleCached ??=
            GetComponentInChildren<FightAnimalAttackModule>(true);
        
        private FightAnimalGetDamageModule _fightAnimalGetDamageModuleCached;
        public FightAnimalGetDamageModule fightAnimalGetDamageModule => _fightAnimalGetDamageModuleCached ??=
            GetComponentInChildren<FightAnimalGetDamageModule>(true);
        
        private FightAnimalFollowModule _fightAnimalFollowModuleCached;
        public FightAnimalFollowModule fightAnimalFollowModule => _fightAnimalFollowModuleCached ??=
            GetComponentInChildren<FightAnimalFollowModule>(true);
        
        private bool _isActive;
        public bool isActive => _isActive;
        
        public readonly TheSignal onActivate = new();
        public readonly TheSignal onDeactivate = new();

        public void Deactivate()
        {
            _isActive = false;
            healthAnimalModule.DeactivateHealth();
            fightAnimalAttackModule.EndAttack();
            fightAnimalFollowModule.EndFollow(false);
            SetIdle();
            onDeactivate.Dispatch();
            
            gameObject.SetActive(false);
        }
        
        public void Activate()
        {
            _isActive = true;
            gameObject.SetActive(true);
            healthAnimalModule.ResetHealth();
            walkRandomModule.StartWalkRandomly();
            fightAnimalGetDamageModule.ResetModule();
            taskModule.aiPath.canMove = true;
            taskModule.aiPath.enableRotation = true;
            SetIdle();
            animator.Rebind();

            fightAnimalGetDamageModule.onTurnOff.Once(() =>
            {
                fightAnimalFollowModule.EndFollow(false);
                fightAnimalAttackModule.EndAttack();
                taskModule.aiPath.canMove = false;
                taskModule.aiPath.enableRotation = false;
            });
            
            onActivate.Dispatch();
        }
    }
}