using UnityEngine;

namespace GameCore.PiggyBank.Misc
{
    public class WorldRotateSetter : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationToSet = Vector3.zero;
        private void OnEnable()
        {
            transform.rotation = Quaternion.Euler(_rotationToSet);
        }
    }
}