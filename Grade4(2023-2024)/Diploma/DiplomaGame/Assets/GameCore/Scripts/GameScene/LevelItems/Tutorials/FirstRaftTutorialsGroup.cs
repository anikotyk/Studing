using GameCore.Common.Misc;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.SaveProperties.Api;
using Zenject;

namespace GameCore.GameScene.LevelItems.Tutorials
{
    public class FirstRaftTutorialsGroup : RaftTutorialsGroup
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        
        private TheSaveProperty<int> _activeBuyObjectIndexSaveProperty;
        
        public override void Construct()
        {
            base.Construct();
            
            _activeBuyObjectIndexSaveProperty = new(CommStr.ActiveBuyObjectIndex_Raft);
            
            initializeInOrderController.Add(Initialize, 1000);
        }

        private void Initialize()
        {
            if (_activeBuyObjectIndexSaveProperty.value <= 0)
            {
                StartTutorialsGroup();
            }
        }
    }
}