using UnityEngine;

namespace GameCore.ShipScene.Common
{
    public class StartPositioner : MonoBehaviour
    {
        [SerializeField] private Vector3 _startPosition;
        
        private void Awake()
        {
            transform.localPosition = _startPosition;
        }
    }
}