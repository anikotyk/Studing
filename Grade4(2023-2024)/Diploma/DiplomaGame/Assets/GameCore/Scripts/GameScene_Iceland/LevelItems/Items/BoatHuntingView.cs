using GameCore.GameScene_Iceland.LevelItems.Items.Modules;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using UnityEngine;

namespace GameCore.GameScene_Iceland.LevelItems.Items
{
    public class BoatHuntingView : InteractorCharacterView
    {
        [SerializeField] private Transform _visual;
        public Transform visual => _visual;
        private SwimHuntingAttackModule _swimHuntingAttackModuleCached;
        public SwimHuntingAttackModule swimHuntingAttackModule => _swimHuntingAttackModuleCached ??=
            GetComponentInChildren<SwimHuntingAttackModule>(true);
        
        private MainCharacterSimpleWalkerController _walkerControllerCached;
        public MainCharacterSimpleWalkerController walkerController => _walkerControllerCached ??=
            GetComponent<MainCharacterSimpleWalkerController>();

        public void ResetView()
        {
            swimHuntingAttackModule.EndAttack();
        }
    }
}