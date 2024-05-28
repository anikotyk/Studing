using GameCore.GameScene_Iceland.LevelItems.Animals.KillerWhale;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.GameScene_Iceland.LevelItems.Items.Modules
{
    public class SwimHuntingAttackModule : InjCoreMonoBehaviour
    {
        [SerializeField] private SwimHunter[] _hunters;
        
        public void StartAttack(WhaleKillerView whaleKillerView)
        {
            float delay = 0f;
            foreach (var hunter in _hunters)
            {
                hunter.StartAttack(whaleKillerView, delay);
                delay += 0.35f;
            }
        }
        
        public void EndAttack()
        {
            foreach (var hunter in _hunters)
            {
                hunter.EndAttack();
            }
        }
    }
}