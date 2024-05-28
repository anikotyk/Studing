using DG.Tweening;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.GameScene_Iceland.LevelItems.Animals.KillerWhale.Modules
{
    public class WhaleKillerGetDamageModule : InjCoreMonoBehaviour
    {
        private bool _isColorTweenPlaying;
        private bool _isEnabled = true;
        public bool isEnabled => _isEnabled;

        private WhaleKillerView _viewCached;
        public WhaleKillerView view => _viewCached ??= GetComponentInParent<WhaleKillerView>(true);

        public void DisableModule()
        {
            _isEnabled = false;
            view.healthAnimalModule.DeactivateHealth(true);
        }
        
        public void EnableModule()
        {
            _isEnabled = true;
            view.healthAnimalModule.ActivateHealth(true);
        }

        private void GetDamage()
        {
            view.healthAnimalModule.GetDamage();
        }

        public void OnHit()
        {
            GetDamage();
            DamageEffects();
        }
        
        private void DamageEffects()
        {
            void BlinkRed(Material[] mats)
            {
                foreach (var mat in mats)
                {
                    Color col = mat.color;
                    mat.DOColor(Color.red, 0.15f).OnComplete(() =>
                    {
                        mat.DOColor(col, 0.15f).SetLink(gameObject).OnComplete(() =>
                        {
                            _isColorTweenPlaying = false;
                        });
                    }).SetLink(gameObject);
                }
            }

            if (!_isColorTweenPlaying)
            {
                _isColorTweenPlaying = true;
                SkinnedMeshRenderer[] characterSkinnedRenderers = view.GetComponentsInChildren<SkinnedMeshRenderer>();
                MeshRenderer[] characterRenderers = view.GetComponentsInChildren<MeshRenderer>();
                foreach (var renderer in characterSkinnedRenderers)
                {
                    Material[] mats = renderer.materials;
                    BlinkRed(mats);
                }
                foreach (var renderer in characterRenderers)
                {
                    Material[] mats = renderer.materials;
                    BlinkRed(mats);
                }
            }
        }
    }
}