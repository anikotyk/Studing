using GameCore.Common.LevelItems.Character.CharacterTyping;
using GameBasicsCore.Game.SaveProperties.SaveData.Base;
using GameBasicsSDK.GameBasicsCore.Plugins.EasySave3.GameBasics;

namespace GameCore.Common.Saves
{
    public class CharacterTypeSaveData : ES3SavePropertyData<CharacterTypeSaveData.InternalSaveData>
    {
        public override string key => "CharacterType";
        
        [ES3Alias("CharacterType")]
        public class InternalSaveData : ES3SaveData<InternalSaveData>
        {
            [ES3Serializable] private CharacterType.Type _type;
            public CharacterType.Type type
            {
                get => _type;
                set
                {
                    _type = value;
                    DispatchChange();
                }
            }
        }
    }
}