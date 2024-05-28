using DG.Tweening;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using UnityEngine;

namespace GameCore.Common.LevelItems.Character.Modules
{
    public class GetDamageCharacterModule : InteractorCharacterModule
    {
        public virtual void GetDamage()
        {
            DamageEffects();
        }

        private void DamageEffects()
        {
            character.transform.DOShakeRotation(0.25f, 30F).SetLink(gameObject);
            void BlinkRed(Material[] mats)
            {
                foreach (var mat in mats)
                {
                    Color col = mat.color;
                    mat.DOColor(Color.red, 0.15f).OnComplete(() =>
                    {
                        mat.DOColor(col, 0.15f).SetLink(gameObject);
                    }).SetLink(gameObject);
                }
            }
            
            SkinnedMeshRenderer[] characterSkinnedRenderers = character.GetComponentsInChildren<SkinnedMeshRenderer>();
            MeshRenderer[] characterRenderers = character.GetComponentsInChildren<MeshRenderer>();
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