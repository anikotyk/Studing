using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;

namespace GameCore.GameScene.LevelItems.Managers
{
    public class BuyObjectsRaftManager : BuyObjectsManager
    {
        protected override void GetSaves()
        {
            _activeBuyObjectIndexSaveProperty = new(CommStr.ActiveBuyObjectIndex_Raft, linkToDispose: gameObject);
            _watchedCutsceneSaveProperty = new(CommStr.WatchedCutsceneArrive_Raft);
        }
    }
}