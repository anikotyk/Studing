using EPOOutline;
using GameCore.GameScene_Island.LevelItems.Platforms;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class AnimalTrap : InjCoreMonoBehaviour
    {
        [SerializeField] private Transform _animalPoint;
        [SerializeField] private AnimatorParameterApplier _closeTrapAnim;
        [SerializeField] private ParticleSystem _vfxOnCloseTrap;
        [SerializeField] private AnimatorParameterApplier _openTrapAnim;
        [SerializeField] private SellProductsCollectPlatform _sellProductsCollectPlatform;
        public SellProductsCollectPlatform sellProductsCollectPlatform => _sellProductsCollectPlatform;
        [SerializeField] private Outlinable[] _outlinables;
        public Transform animalPoint => _animalPoint;

        private bool _isCharged;
        public bool isCharged => _isCharged;
        
        public readonly TheSignal onCharged = new();
        public override void Construct()
        {
            base.Construct();

            _sellProductsCollectPlatform.onFull.On(() =>
            {
                if(_isCharged) return;
                _isCharged = true;
                onCharged.Dispatch();
            });
        }

        public void CloseTrap()
        {
            _sellProductsCollectPlatform.interactItem.enabled = false;
            _isCharged = false;
            _closeTrapAnim.Apply();
            _vfxOnCloseTrap.Play();
            _sellProductsCollectPlatform.RemoveAllProducts();
            foreach (var outlinable in _outlinables)
            {
                outlinable.enabled = true;
            }
        }
        
        public void ResetTrap()
        {
            _sellProductsCollectPlatform.interactItem.enabled = true;
            _isCharged = false;
            _openTrapAnim.Apply();
            foreach (var outlinable in _outlinables)
            {
                outlinable.enabled = false;
            }
        }
    }
}