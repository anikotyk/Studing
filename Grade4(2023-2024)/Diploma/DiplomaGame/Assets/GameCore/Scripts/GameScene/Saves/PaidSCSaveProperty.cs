using GameBasicsCore.Game.SaveProperties.Api;

namespace GameCore.GameScene.Saves
{
    public class PaidSCSaveProperty : ES3SaveProperty<int>
    {
        public override string key => "PaidSC";
        public override int defaultValue => 0;
    }
}