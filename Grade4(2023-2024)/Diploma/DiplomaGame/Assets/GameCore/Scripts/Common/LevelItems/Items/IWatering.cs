
using UnityEngine;

namespace GameCore.Common.LevelItems.Items
{
    public interface IWatering
    {
        public void OnWatered();
        public Transform pointWatering { get; }
    }
}