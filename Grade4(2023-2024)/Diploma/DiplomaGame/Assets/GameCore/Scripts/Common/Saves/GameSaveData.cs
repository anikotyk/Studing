using GameBasicsCore.Game.SaveProperties.SaveData.Base;
using GameBasicsSDK.GameBasicsCore.Plugins.EasySave3.GameBasics;

namespace GameCore.Common.Saves
{
    public class GameSaveData : ES3SavePropertyData<GameSaveData.InternalSaveData>
    {
        public override string key => "GameData";
        
        [ES3Alias("GameData")]
        public class InternalSaveData : ES3SaveData<InternalSaveData>
        {
            [ES3Serializable] private string _lastPlayedScene;
            public string lastPlayedScene
            {
                get => _lastPlayedScene;
                set
                {
                    _lastPlayedScene = value;
                    DispatchChange();
                }
            }
        }
    }
}