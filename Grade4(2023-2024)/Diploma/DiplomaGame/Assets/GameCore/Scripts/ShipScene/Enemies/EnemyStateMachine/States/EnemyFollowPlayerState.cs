using System;
using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.WalkerControllers;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Enemies.EnemyStateMachine
{
    public class EnemyFollowPlayerState : EnemyState
    {
        private void Update()
        {
            if(isEntered == false)
                return;
            enemy.aiPath.destination = enemy.mainCharacterView.transform.position;
        }
    }
}