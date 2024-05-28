using System;
using UnityEngine;

namespace GameCore.ShipScene.Products
{
    public class ProductsClaimer : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ProductClaimItem claimItem))
            {
                claimItem.Claim();
            }
        }
    }
}