using DG.Tweening;
using UnityEngine;

namespace GameCore.ShipScene.Weapons.Fx
{
    public class CrossbowFx : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private SkinnedMeshRenderer _pope;
        [SerializeField] private float _shotDuration;
        [SerializeField] private float _pullDuration;

        private void OnEnable()
        {
            _weapon.shootingStarted.On(OnShootingStarted);
            _weapon.shot.On(OnShot);
        }

        private void OnDisable()
        {
            _weapon.shootingStarted.Off(OnShootingStarted);
            _weapon.shot.Off(OnShot);
        }

        private void Shot()
        {
            DOTween.Kill(this);
            DOVirtual.Float(0, 100, _shotDuration, value => _pope.SetBlendShapeWeight(0, value)).SetId(this);
        }

        private void Pull()
        {
            DOTween.Kill(this);
            DOVirtual.Float( _pope.GetBlendShapeWeight(0), 0, _pullDuration, value => _pope.SetBlendShapeWeight(0, value)).SetId(this);
        }
        
        private void OnShootingStarted(Transform target)
        {
            Pull();
        }

        private void OnShot()
        {
            Shot();
        }

    }
}