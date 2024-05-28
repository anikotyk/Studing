using GameCore.Common.LevelItems;
using UnityEngine;

namespace GameCore.GameScene.LevelItems.Tutorials
{
    public class BuyRaftTutorial : RaftTutorial
    {
        [SerializeField] private BuyObject _object;
        public override Vector3 targetPos => _object.transform.position + Vector3.up * 0.5f;
        
        public override void Construct()
        {
            base.Construct();

            _object.onBuy.Once(CompleteTutorial);
        }
    }
}