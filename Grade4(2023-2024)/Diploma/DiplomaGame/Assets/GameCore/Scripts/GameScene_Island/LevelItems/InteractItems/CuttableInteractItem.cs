using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene_Island.LevelItems.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene_Island.LevelItems.InteractItems
{
    public class CuttableInteractItem : InteractItem
    {
        public override int priority { get; } = 4;
        
        private CuttableItem _cuttableItemCached;

        public CuttableItem cuttableItem
        {
            get
            {
                if (_cuttableItemCached == null) _cuttableItemCached = GetComponent<CuttableItem>();
                return _cuttableItemCached;
            }
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (interactorModel.view.GetModule<CuttingModule>() && interactorModel.view.GetModule<CuttingModule>().CanCutNow(cuttableItem.spawnProductConfig))
            {
                //cuttableItem.Cut(interactorModel is MainCharacterModel);
            }
        }
    }
}