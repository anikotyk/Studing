using DG.Tweening;
using GameCore.Common.LevelItems;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Misc
{
    public class ColliderDisabler : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [SerializeField] private GameObject _collider;

        private bool _canDisable;

        public override void Construct()
        {
            base.Construct();
            initializeInOrderController.Add(Initialize, 2000);
        }

        private void Initialize()
        {
            gameObject.SetActive(false);
            DOVirtual.DelayedCall(0.05f, () =>
            {
                _canDisable = true;
                gameObject.SetActive(true);
            });
        }

        private void DisableCollider()
        {
            _collider.SetActive(false);
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            CheckForDisable(collision);
        }
        
        private void OnCollisionStay(Collision collision)
        {
            CheckForDisable(collision);
        }

        private void CheckForDisable(Collision collision)
        {
            if (_canDisable && collision.transform.GetComponent<ColliderDisabler>())
            {
                var buyObject = GetComponentInParent<BuyObject>(true);
                if (!buyObject || buyObject.isBought)
                {
                    DisableCollider();
                }
            }
        }
    }
}