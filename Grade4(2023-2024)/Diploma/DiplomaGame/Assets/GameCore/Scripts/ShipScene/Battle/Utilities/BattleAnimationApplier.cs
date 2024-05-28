using GameCore.ShipScene.Battle.Waves;
using GameCore.ShipScene.Extentions;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.ShipScene.Battle.Utilities
{
    public class BattleAnimationApplier : BattleStartEndListener
    {
        [SerializeField] private ReversibleAnimatorApplier _reversibleAnimatorApplier;
        [SerializeField] private bool _apply;
        [SerializeField] private bool _revert;
        [SerializeField] private bool _invert;

        public override void Construct()
        {
            base.Construct();
            _reversibleAnimatorApplier.Revert();
        }

        protected override void OnBattleStarted(Wave wave)
        {
            if(_apply && _invert == false)
                _reversibleAnimatorApplier.Apply();
            if(_revert && _invert)
                _reversibleAnimatorApplier.Revert();
        }

        protected override void OnBattleEnded(Wave wave)
        {
            if(_revert)
                _reversibleAnimatorApplier.Revert();
            if(_apply && _invert)
                _reversibleAnimatorApplier.Apply();
        }
    }
}