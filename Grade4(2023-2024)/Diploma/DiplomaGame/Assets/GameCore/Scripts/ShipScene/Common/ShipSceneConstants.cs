using GameBasicsCore.Game.Misc;

namespace GameCore.ShipScene.Common
{
    public static class ShipSceneConstants
    {
        public static readonly string saveFile = "ShipSceneSave";
        public static readonly string shipCoinsSaveKey = "ShipCoins";
        public static readonly string saveFilePath = $"{NCStr.Saves}/" + saveFile + ".data";
    }
}