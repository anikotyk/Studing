using System.Collections.Generic;
using DG.Tweening;
using ModestTree;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using UnityEngine;

namespace GameCore.Common.Misc
{
    public class BecomeInvisibleObject : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _renderers;
        [SerializeField] private float _fadeTime = 0.5f;
        private List<Collider> _triggers = new List<Collider>();
        private bool _isInvisible;
        private Sequence _visibleSequence;

        private void OnTriggerEnter(Collider other)
        {
            if (IsCorrectTrigger(other))
            {
                if (!_triggers.Contains(other))
                {
                    _triggers.Add(other);
                    if (!_isInvisible) BecomeInvisible();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_triggers.Contains(other))
            {
                _triggers.Remove(other);
                if(_isInvisible && _triggers.IsEmpty()) BecomeVisible();
            }
        }

        private bool IsCorrectTrigger(Collider other)
        {
            return other.GetComponent<MainCharacterView>();
        }
        
        private void BecomeInvisible()
        {
            _isInvisible = true;

            if (_visibleSequence != null)
            {
                _visibleSequence.Kill();
            }

            _visibleSequence = DOTween.Sequence().SetLink(gameObject);
            foreach (var renderer in _renderers)
            {
                foreach (var mat in renderer.materials)
                {
                    _visibleSequence.Join(mat.DOFade(0.35f, _fadeTime).SetLink(gameObject)).SetLink(gameObject);
                }
            }
        }
        
        private void BecomeVisible()
        {
            _isInvisible = false;
            
            if (_visibleSequence != null)
            {
                _visibleSequence.Kill();
            }

            _visibleSequence = DOTween.Sequence().SetLink(gameObject);
            
            foreach (var renderer in _renderers)
            {
                foreach (var mat in renderer.materials)
                {
                    _visibleSequence.Join(mat.DOFade(1, _fadeTime).SetLink(gameObject)).SetLink(gameObject);
                }
            }
        }
    }
}