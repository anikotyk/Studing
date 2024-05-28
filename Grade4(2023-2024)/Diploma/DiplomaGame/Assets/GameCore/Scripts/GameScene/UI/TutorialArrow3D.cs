using DG.Tweening;
using GameCore.GameScene.LevelItems.Tutorials;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc.Refs;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.UI
{
    public class TutorialArrow3D : InjCoreMonoBehaviour
    {
        [SerializeField] private Transform _arrow;
        [SerializeField] private Transform _arrowVisible;
        [Inject, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        [Inject, UsedImplicitly] public MainCameraRef mainCameraRef { get; }
        private MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        
        private RaftTutorial _raftTutorial;
        public Vector3 targetPos => _raftTutorial.targetPos;
        private bool _isShown = false;
        
        private bool _isEnabled = false;
        private bool _savedIsEnabled;

        private Tween _scaleTween;
        private Tween _scaleZTween;

        public void SetTarget(RaftTutorial raftTutorial)
        {
            _isEnabled = true;
            _savedIsEnabled = true;
            _raftTutorial = raftTutorial;
            Show();
            AnimateArrow();
        }
        
        public void RemoveTarget()
        {
            _isEnabled = false;
            _savedIsEnabled = false;
            Hide();
        }

        private void Update()
        {
            if(!mainCharacterView) return;
            if(!_isEnabled) return;
            if(!_raftTutorial) return;

            ValidateArrow();
        }

        private void ValidateArrow()
        {
            Vector3 dir = (targetPos - mainCharacterView.transform.position);
            Vector3 dirNormalized = dir.normalized;
            dirNormalized = Vector3.ProjectOnPlane(dirNormalized, Vector3.up);
            if (dirNormalized.magnitude > 0.01f)
            {
                _arrow.forward = dirNormalized * -1;
            }
            
            if (dir.magnitude < 1f)
            {
                if (_isShown)
                {
                    Hide();
                }
            }
            else
            {
                if (!_isShown)
                {
                    Show();
                }
            }
            
        }
        
        private void Show()
        {
            if(_isShown) return;
            _isShown = true;
            _arrowVisible.gameObject.SetActive(true);
            
            if (_scaleTween != null)
            {
                _scaleTween.Kill();
            }

            _arrowVisible.transform.localScale = Vector3.zero;
            _scaleTween = _arrowVisible.DOScale(Vector3.one, 0.25f).SetLink(gameObject);
        }
        
        private void Hide()
        {
            _isShown = false;
            
            if (_scaleTween != null)
            {
                _scaleTween.Kill();
            }
            
            _scaleTween = _arrowVisible.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
            {
                _arrowVisible.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }

        private void AnimateArrow()
        {
            if (_scaleZTween != null && _scaleZTween.IsPlaying()) return;
            _scaleZTween = _arrow.DOScaleZ(0.7f, 0.35f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        }
        
        private void StopAnimateArrow()
        {
            if (_scaleZTween != null)
            {
                _scaleZTween.Kill();
                _arrow.localScale = Vector3.one;
            }
        }

        public void Enable()
        {
            _isEnabled = _savedIsEnabled;
        }
        
        public void Disable()
        {
            _savedIsEnabled = _isEnabled;
            _isEnabled = false;
            Hide();
        }
    }
}