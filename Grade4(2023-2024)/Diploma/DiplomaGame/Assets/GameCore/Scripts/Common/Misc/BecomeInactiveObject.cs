using System.Collections.Generic;
using ModestTree;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using UnityEngine;

namespace GameCore.Common.Misc
{
    public class BecomeInactiveObject : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _meshRenderers;
        private List<Collider> _triggers = new List<Collider>();
        private bool _isInactive;

        private void OnTriggerEnter(Collider other)
        {
            if (IsCorrectTrigger(other))
            {
                if (!_triggers.Contains(other))
                {
                    _triggers.Add(other);
                    if (!_isInactive) BecomeInactive();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_triggers.Contains(other))
            {
                _triggers.Remove(other);
                if(_isInactive && _triggers.IsEmpty()) BecomeActive();
            }
        }

        private bool IsCorrectTrigger(Collider other)
        {
            return other.GetComponent<MainCharacterView>();
        }
        
        private void BecomeInactive()
        {
            _isInactive = true;

            foreach (var renderer in _meshRenderers)
            {
                renderer.enabled = false;
            }
        }
        
        private void BecomeActive()
        {
            _isInactive = false;
            
            foreach (var renderer in _meshRenderers)
            {
                renderer.enabled = true;
            }
        }
    }
}