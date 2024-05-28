using System.Collections.Generic;
using DG.Tweening;
using GameCore.GameScene.Settings;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using UnityEngine;

namespace GameCore.GameScene.Misc
{
    public class TileMoveDown : MonoBehaviour
    {
        [SerializeField] private Transform _moveObject;
        
        private float _moveDownPosY => GameplaySettings.def.raftsMoveDownUnderCharacterYPos;

        private List<CharacterView> _views = new List<CharacterView>();
        private bool _isOnDown = false;
        private Tween _tween;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<CharacterView>(out CharacterView view))
            {
                if (_views.Contains(view)) return;
                _views.Add(view);
                
                if(_isOnDown) return;
                
                if (_tween != null)
                {
                    _tween.Kill();
                }
                _tween = _moveObject.DOLocalMoveY(_moveDownPosY, 0.25f).SetLink(gameObject);
                _isOnDown = true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if(!_isOnDown) return;
            if (other.TryGetComponent<CharacterView>(out CharacterView view))
            {
                if (!_views.Contains(view)) return;
                _views.Remove(view);
                
                if (_views.Count > 0) return;
                
                if (_tween != null)
                {
                    _tween.Kill();
                }
                _tween = _moveObject.DOLocalMoveY(0, 0.25f).SetLink(gameObject);
                _isOnDown = false;
            }
        }
    }
}