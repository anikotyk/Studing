using DG.Tweening;
using GameBasicsCore.Game.Misc;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class AnimatedProductionItem : ProductionItem
    {
        [SerializeField] private AnimatorParameterApplier[] _workingAnims;
        [SerializeField] private AnimatorParameterApplier[] _stopWorkingAnims;
        [SerializeField] private AnimatorParameterApplier _spawnAnim;
        [SerializeField] private float _spawnAnimTime;
        [SerializeField] private ParticleSystem _vfxWorking;
        [SerializeField] private GameObject[] _activateWhileWork;
        
        protected override void EffectOnStartWorking()
        {
            foreach (var anim in _workingAnims)
            {
                anim.Apply();
            }

            foreach (var obj in _activateWhileWork)
            {
                obj.gameObject.SetActive(true);
            }
            if(_vfxWorking != null) _vfxWorking.Play();
        }
        
        protected override void EffectOnEndWorking()
        {
            foreach (var anim in _stopWorkingAnims)
            {
                anim.Apply();
            }
            foreach (var obj in _activateWhileWork)
            {
                obj.gameObject.SetActive(false);
            }
            if(_vfxWorking != null) _vfxWorking.Stop();
        }
        
        protected override void SpawnProduct()
        {
            if (_spawnAnim != null)
            {
                _spawnAnim.Apply();
                DOVirtual.DelayedCall(_spawnAnimTime, () =>
                {
                    base.SpawnProduct();
                }, false).SetLink(gameObject);
            }
            else
            {
                base.SpawnProduct();
            }
        }
    }
}