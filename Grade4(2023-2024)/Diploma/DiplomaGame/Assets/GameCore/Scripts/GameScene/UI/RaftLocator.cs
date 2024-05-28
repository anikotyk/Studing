using DG.Tweening;
using GameCore.GameScene.LevelItems;
using JetBrains.Annotations;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc.Refs;
using GameBasicsSDK.Modules.IdleArcade.Controllers.SceneContext;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.UI
{
    public class RaftLocator : InjCoreMonoBehaviour
    {
        [InjectOptional, UsedImplicitly] public MainCameraRef mainCameraRef { get; }
        public Camera cam => mainCameraRef.camera;
        [InjectOptional, UsedImplicitly] public InteractorCharactersCollection interactorCharactersCollection { get; }
        private MainCharacterView mainCharacterView => interactorCharactersCollection.mainCharacterView;
        [InjectOptional, UsedImplicitly] public Raft raft { get; }
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _locator;
        [SerializeField] private Transform _arrow;
        
        private bool _isShown;
        private Tween _alphaTween;
        
        private void Update()
        {
            if(!mainCharacterView || !raft || !cam) return;
            
            var cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);
            if (GeometryUtility.TestPlanesAABB(cameraFrustum, raft.bounds))
            {
                if (_isShown)
                {
                    HideLocator();
                }
            }
            else
            {
                if (!_isShown)
                {
                    ShowLocator();
                }

                ValidateLocator();
            }
        }

        private void ValidateLocator()
        {
            Vector3 dir = (raft.bounds.center - mainCharacterView.transform.position).normalized;
            dir = Vector3.ProjectOnPlane(dir, Vector3.up);
            Vector3 dirUI = new Vector3(dir.x, dir.z, 0);
            _locator.transform.localPosition = dirUI * 200f;
            
            Vector3 rotateUI = new Vector3(0, 0, Vector3.SignedAngle(Vector3.up, dirUI, Vector3.forward));
            _arrow.localRotation = Quaternion.Euler(rotateUI);
        }
        
        private void ShowLocator()
        {
            _isShown = true;
            _locator.SetActive(true);
            
            if (_alphaTween != null)
            {
                _alphaTween.Kill();
            }
            _alphaTween = _canvasGroup.DOFade(1, 0.25f).SetLink(gameObject);
        }
        
        private void HideLocator()
        {
            _isShown = false;
            
            if (_alphaTween != null)
            {
                _alphaTween.Kill();
            }
            _alphaTween = _canvasGroup.DOFade(0, 0.25f).OnComplete(() =>
            {
                _locator.SetActive(false);
            }).SetLink(gameObject);
        }
    }
}