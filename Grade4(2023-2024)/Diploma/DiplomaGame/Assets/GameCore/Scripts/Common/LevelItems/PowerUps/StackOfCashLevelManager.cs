using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCore.Common.LevelItems.PowerUps
{
    public class StackOfCashLevelManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> _points;

        public Vector3 GetPosition()
        {
            int index = Random.Range(0, _points.Count);
            return _points[index].position;
        }
    }
}
