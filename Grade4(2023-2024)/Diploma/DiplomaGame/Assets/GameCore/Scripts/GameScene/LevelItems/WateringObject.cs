using GameCore.Common.LevelItems;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.GameScene.LevelItems
{
    public class WateringObject : InjCoreMonoBehaviour
    {
        [SerializeField] private ShowerObject[] _showers;

        public void ShowWatering()
        {
            foreach (var shower in _showers)
            {
                shower.StartShower();
            }
        }
    }
}