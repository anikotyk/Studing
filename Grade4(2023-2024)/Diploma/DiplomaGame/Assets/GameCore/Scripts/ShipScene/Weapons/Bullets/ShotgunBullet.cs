 using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace GameCore.ShipScene.Weapons.Bullets
{
    public class ShotgunBullet : Bullet
    {
        [SerializeField] private float _spreadSpeed;

        private Coroutine _spreadRoutine;
        protected override void OnShootStart(Transform target)
        {
            foreach (var projectile in projectiles)
                projectile.transform.localPosition = Vector3.zero;
            
            _spreadRoutine = StartCoroutine(Spread());
        }

        private IEnumerator Spread()
        {
            float speed = Time.fixedDeltaTime * _spreadSpeed;
            while (gameObject.activeInHierarchy)
            {
                int firstTarget = projectiles.Length / 2;
                for (int i = 0; i <= firstTarget ; i++)
                    projectiles[i].transform.localPosition += Vector3.right * (speed * i );
                for (int i = firstTarget; i < projectiles.Length; i++)
                    projectiles[i].transform.localPosition += Vector3.left * (speed * (i - firstTarget));
                yield return new WaitForFixedUpdate();
            }
        }

        protected override void OnShootEnd()
        {
            if(_spreadRoutine != null)
                StopCoroutine(_spreadRoutine);
        }
    }
}