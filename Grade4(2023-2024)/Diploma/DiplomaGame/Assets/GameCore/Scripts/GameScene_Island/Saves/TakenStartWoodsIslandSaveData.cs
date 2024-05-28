using System.Collections.Generic;
using GameBasicsCore.Game.SaveProperties.SaveData.Base;
using GameBasicsSDK.GameBasicsCore.Plugins.EasySave3.GameBasics;

namespace GameCore.GameScene_Island.Saves
{
    public class TakenStartWoodsIslandSaveData : ES3SavePropertyData<TakenStartWoodsIslandSaveData.InternalSaveData>
    {
        public override string key => "TakenStartWoodsIsland";

        [ES3Alias("TakenStartWoodsIsland")]
        public class InternalSaveData : ES3SaveData<InternalSaveData>
        {
            [ES3Serializable] private List<int> _used = new List<int>();

            public bool IsUsed(int index) => _used != null && _used.Contains(index);

            public void SetUsed(int index)
            {
                _used ??= new();
                if (_used.Contains(index)) return;
                _used.Add(index);
                DispatchChange();
            }
        }
    }
}