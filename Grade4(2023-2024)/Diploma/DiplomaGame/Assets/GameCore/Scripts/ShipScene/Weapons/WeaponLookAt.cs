using StaserSDK.Extentions;
using UnityEngine;

namespace GameCore.ShipScene.Weapons
{
    public class WeaponLookAt : MonoBehaviour
    {
        [SerializeField] private Transform _lookAtTarget;
        [SerializeField] private float _lookAtSpeed;

        private void Update()
        {
            Vector3 relativePos = _lookAtTarget.position - transform.position;
            Quaternion rotation = Quaternion.Inverse(transform.parent.rotation) * Quaternion.LookRotation(relativePos);
            rotation = rotation.Multiply(Vector3.right);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, _lookAtSpeed * Time.deltaTime);
        }
    }
}