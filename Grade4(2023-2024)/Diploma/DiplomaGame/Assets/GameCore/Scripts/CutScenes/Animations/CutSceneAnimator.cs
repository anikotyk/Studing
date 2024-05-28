using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameCore.CutScenes.Animations
{
    public class CutSceneAnimator : MonoBehaviour
    {
        [SerializeField] private List<CutSceneAnimationItem> _animationItems;

        private int _currentAnimationIndex = 0;
        
        private void OnEnable()
        {
            if(_animationItems.Count == 0)
                return;
            _currentAnimationIndex = 0;
            MoveNext();
        }

        private void MoveNext()
        {
            if(_currentAnimationIndex >= _animationItems.Count)
                return;
            int index = _currentAnimationIndex;
            _currentAnimationIndex++;
            _animationItems[index].Animate().OnComplete(MoveNext);
        }
    }
}