using GameBasicsCore.Game.SaveProperties.Api;

namespace GameCore.GameScene.Saves
{
    public class PaidWoodSaveProperty : ES3SaveProperty<int>
    {
        public override string key => "PaidWood";
        public override int defaultValue => 0;
    }
}