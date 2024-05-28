using System;
using System.Collections.Generic;
using GameCore.ShipScene.Battle.Waves;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.ShipScene.Battle.Utilities
{
    public class BattleActiveChanger : BattleStartEndListener
    {
        [SerializeField] private List<GameObject> _objects;
        [SerializeField] private bool _targetActive;

        private void OnEnable()
        {
            _objects.ForEach(x=> x.SetActive(!_targetActive));
        }

        protected override void OnBattleStarted(Wave wave)
        {
            _objects.ForEach(x=> x.SetActive(_targetActive));
        }

        protected override void OnBattleEnded(Wave wave)
        {
            _objects.ForEach(x=> x.SetActive(!_targetActive));
        }
    }
}