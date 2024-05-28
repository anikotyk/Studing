using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.GameScene_Iceland.LevelItems.Animals.KillerWhale.Modules
{
    public class WhaleKillerWalkBySplineModule : InjCoreMonoBehaviour
    {
        [SerializeField] private GameObject[] _effectsSwim;
        
        private WhaleKillerView _viewCached;
        public WhaleKillerView view => _viewCached ??= GetComponentInParent<WhaleKillerView>(true);
        
        public void StartWalk()
        {
            view.splineFollower.follow = true;
            
            foreach (var effect in _effectsSwim)
            {
                effect.SetActive(true);
            }
        }

        public void StopWalk()
        {
            view.splineFollower.follow = false;
            
            foreach (var effect in _effectsSwim)
            {
                effect.SetActive(false);
            }
        }
    }
}