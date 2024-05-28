using GameBasicsCore.Game.Misc;
using UnityEngine;

namespace GameCore.ShipScene.Weapons.Fx
{
    public class CannonFx : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private ParticleSystem _attackPrepareParticle;
        [SerializeField] private AnimatorParameterApplier _shotAnimation;

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

        private void OnShootingStarted(Transform target)
        {
            _attackPrepareParticle.Play();
        }

        private void OnShot()
        {
            _attackPrepareParticle.Stop();
            _shotAnimation.Apply();
        }
    }
}