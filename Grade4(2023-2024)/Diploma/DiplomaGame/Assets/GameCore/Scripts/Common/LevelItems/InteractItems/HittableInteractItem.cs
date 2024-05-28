using GameCore.Common.LevelItems.Character.Modules;
using GameCore.Common.LevelItems.Items.HittableItems;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.Common.LevelItems.InteractItems
{
    public class HittableInteractItem : InteractItem
    {
        public override int priority { get; } = 5;
        
        private HittableItem _hittableCached;
        public HittableItem hittable
        {
            get
            {
                if (_hittableCached == null) _hittableCached = GetComponent<HittableItem>();
                return _hittableCached;
            }
        }

        private InteractorCharacterModel _currentInteractor;

        public override bool CanInteract(InteractorCharacterModel interactorModel)
        {
            return base.CanInteract(interactorModel) && interactorModel.view.GetModule<HittingCharacterModule>();
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (!hittable.isEnabled) return;
            
            interactorModel.view.GetModule<HittingCharacterModule>().EnteredHittableItem(hittable);
            _currentInteractor = interactorModel;
        }

        public override void OnExit(InteractorCharacterModel interactorModel)
        {
            interactorModel.view.GetModule<HittingCharacterModule>().ExitedHittableItem(hittable);
            _currentInteractor = null;
        }

        public void ExitCurrentInteractor()
        {
            if (_currentInteractor != null)
            {
                OnExit(_currentInteractor);
            }
        }
    }
}