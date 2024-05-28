using GameCore.ShipScene.Damage;
using GameCore.ShipScene.Products;
using UnityEngine;

namespace GameCore.ShipScene.Enemies.Combat
{
    public class EnemyDeathReward : LiveListener
    {
        [SerializeField] private ProductSpawner _productSpawner;
        [SerializeField] private int _rewardCount;
        
        protected override void OnDied()
        {
            _productSpawner.Spawn(_rewardCount);
        }

        protected override void OnRevived()
        {
        }
    }
}