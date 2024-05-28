using System.Collections.Generic;
using GameBasicsCore.Game.SaveProperties.SaveData.Base;
using GameBasicsSDK.GameBasicsCore.Plugins.EasySave3.GameBasics;

namespace GameCore.GameScene_Island.Saves
{
    public class ShipStagesSaveData : ES3SavePropertyData<ShipStagesSaveData.InternalSaveData>
    {
        public override string key => "ShipStages";

        [ES3Alias("ShipStages")]
        public class InternalSaveData : ES3SaveData<InternalSaveData>
        {
            [ES3Serializable] private List<int> _active = new List<int>();

            public bool IsActive(int index) => _active != null && _active.Contains(index);

            public void SetActive(int index)
            {
                _active ??= new();
                if (_active.Contains(index)) return;
                _active.Add(index);
                DispatchChange();
            }

            public bool IsNoStagesActive() => _active == null || _active.Count <= 0;
        }
    }
}