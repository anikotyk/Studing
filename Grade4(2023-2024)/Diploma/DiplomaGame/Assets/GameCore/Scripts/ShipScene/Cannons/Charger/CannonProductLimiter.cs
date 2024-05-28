using GameCore.ShipScene.Interactions;
using UnityEngine;

namespace GameCore.ShipScene.Cannons
{
    public class CannonProductLimiter : ProductLimiter
    {
        [SerializeField] private Cannon _cannon;

        public override bool isAvailable => _cannon.gameObject.activeInHierarchy;
        public override int limit => _cannon.capacity;
        public override int freeSpace => _cannon.freeSpace;
    }
}