using GameBasicsCore.Game.Controllers;

namespace GameCore.Common.Controllers
{
    public class ProductionController : ControllerInternal
    {
        private float _productionMultiplier = 1;
        public float productionMultiplier => _productionMultiplier;

        public void SetProductionMultiplier(float multiplier)
        {
            _productionMultiplier = multiplier;
        }

        public void ResetProductionMultiplier()
        {
            _productionMultiplier = 1;
        }
    }
}