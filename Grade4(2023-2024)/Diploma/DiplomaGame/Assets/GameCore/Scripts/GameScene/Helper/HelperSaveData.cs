using GameBasicsCore.Game.SaveProperties.SaveData.Base;
using GameBasicsSDK.GameBasicsCore.Plugins.EasySave3.GameBasics;

namespace GameCore.GameScene.Helper
{
    public class HelperSaveData : ES3SavePropertyData<HelperSaveData.InternalSaveData>
    {
        public override string key => "Helper";

        [ES3Alias("HelperSaveData")]
        public class InternalSaveData : ES3SaveData<InternalSaveData>
        {
            [ES3Serializable]
            private bool _isBought = false;
            public bool isBought
            {
                get => _isBought;
                set
                {
                    _isBought = value;
                    DispatchChange();
                }
            }
        }
    }
}