using GameBasicsCore.Game.Controllers;

namespace GameCore.GameScene.Helper
{
    public class HelperController : CoreController
    {
        private HelperModel _model;

        public HelperModel CreateModel(HelperDataConfig dataConfig)
        {
            var model = new HelperModel(dataConfig);
            _model = model;
            return model;
        }

        public HelperModel GetModel(HelperDataConfig dataConfig)
        {
            return _model ?? CreateModel(dataConfig);;
        }
    }
}