using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class BuyObjectsIcelandManager : BuyObjectsManager
    {
        protected override void GetSaves()
        {
            _activeBuyObjectIndexSaveProperty = new(CommStr.ActiveBuyObjectIndex_Iceland, linkToDispose: gameObject);
            _watchedCutsceneSaveProperty = new(CommStr.WatchedCutsceneArrive_Iceland);
        }
    }
}