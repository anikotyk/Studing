using DG.Tweening;
using EPOOutline;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.LevelItems.Products;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Tools.DOTweenAnimations;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Modules
{
    public class AnimalInTrapModule : CoreMonoBehaviour
    {
        [SerializeField] private AnimatorParameterApplier _inTrapAnim;
        [SerializeField] private Outlinable _outlinable;
        [SerializeField] private Color _colorInTrap;
        [SerializeField] private Color _colorDefault;
        [SerializeField] private MeatAnimalItem _meatAnimalItem;
        [SerializeField] private ParticleSystem _vfxOnDie;
        [SerializeField] private AudioSource _soundInTrap;
        
        private HogView _viewCached;
        public HogView view => _viewCached ??= GetComponentInParent<HogView>();

        private Tween _pulseTween;
        
        public readonly TheSignal onDead = new();
        
        public void SetInTrap(AnimalTrap trap)
        {
            view.taskModule.aiPath.canMove = false;
            _inTrapAnim.Apply();
            DOVirtual.DelayedCall(0.1f, () =>
            {
                _outlinable.OutlineParameters.Color = _colorInTrap;
                _soundInTrap.Play();
            }, false).SetLink(gameObject);
            
            _pulseTween = view.transform.DOPulseScaleDefault(1f, 1.025f, 0.5f)
                .SetLink(gameObject)
                .SetLoops(-1);

            _meatAnimalItem.transform.localScale = Vector3.one;
            _meatAnimalItem.Activate();
            _meatAnimalItem.interactItem.enabled = true;
            _meatAnimalItem.onDead.Once(() =>
            {
                trap.ResetTrap();
                OnDie();
            });
        }

        private void OnDie()
        {
            if (_pulseTween != null)
            {
                _pulseTween.Kill();
            }
            _outlinable.OutlineParameters.Color = _colorDefault;
            _outlinable.enabled = true;
            var vfxCopy = Instantiate(_vfxOnDie);
            vfxCopy.transform.position = _vfxOnDie.transform.position;
            vfxCopy.Play();
            
            onDead.Dispatch();
        }
    }
}