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
    public class TutorialArrow : InjCoreMonoBehaviour
    {
        [InjectOptional, UsedImplicitly] public MainCameraRef mainCameraRef { get; }
        [InjectOptional, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        public MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        
        [SerializeField] private RectTransform _canvas;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _arrow;
        
        public Camera cam => mainCameraRef.camera;
        
        private RaftTutorial _raftTutorial;
        public Vector3 targetPos => _raftTutorial.targetPos;
        private bool _isShown;
        private bool _isEnabled;

        private Tween _alphaTween;
        private Tween _scaleYTween;

        public void SetTarget(RaftTutorial raftTutorial)
        {
            _isEnabled = true;
            _raftTutorial = raftTutorial;
            Show();
            AnimateArrow();
        }
        
        public void RemoveTarget()
        {
            Hide();
            _isEnabled = false;
        }

        private void Update()
        {
            if(!mainCharacterView) return;
            if(!_isEnabled) return;
            if(!_raftTutorial) return;
            if(!cam) return;

            ValidateArrow();
        }

        private void ValidateArrow()
        {
            Vector3 viewPos = cam.WorldToViewportPoint(targetPos);
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
            {
                Vector2 screenPos = new Vector2(
                    ((viewPos.x*_canvas.sizeDelta.x)-(_canvas.sizeDelta.x*0.5f)),
                    ((viewPos.y*_canvas.sizeDelta.y)-(_canvas.sizeDelta.y*0.5f)));

                _arrow.GetComponent<RectTransform>().anchoredPosition = screenPos;
                _arrow.localRotation = Quaternion.Euler(Vector3.zero);
                if (!_isShown)
                {
                    Show();
                }
            }
            else
            {
                if (_isShown)
                {
                    Hide();
                }
            }
        }
        
        private void Show()
        {
            _isShown = true;
            _arrow.gameObject.SetActive(true);
            
            if (_alphaTween != null)
            {
                _alphaTween.Kill();
            }
            _alphaTween = _canvasGroup.DOFade(1, 0.25f).SetLink(gameObject);
        }
        
        private void Hide()
        {
            _isShown = false;
            
            if (_alphaTween != null)
            {
                _alphaTween.Kill();
            }
            _alphaTween = _canvasGroup.DOFade(0, 0.25f).OnComplete(() =>
            {
                _arrow.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }

        private void AnimateArrow()
        {
            if (_scaleYTween != null && _scaleYTween.IsPlaying()) return;
            _scaleYTween = _arrow.DOScaleY(0.7f, 0.35f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        }
        
        private void StopAnimateArrow()
        {
            if (_scaleYTween != null)
            {
                _scaleYTween.Kill();
                _arrow.localScale = Vector3.one;
            }
        }
    }
}