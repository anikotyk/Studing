using System;
using DG.Tweening;
using UnityEngine;

namespace GameCore.ShipScene.Common
{
    public class OnEnableAstarScaner : MonoBehaviour
    {
        [SerializeField] private float _updateDelay;

        private void OnEnable()
        {
            DOVirtual.DelayedCall(_updateDelay, () => AstarPath.active.Scan()).SetId(this);
        }

        private void OnDisable()
        {
            DOTween.Kill(this);
        }
    }
}