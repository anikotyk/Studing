using System.Collections.Generic;
using DG.Tweening;
using GameCore.GameScene_Island.LevelItems.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.Models;

namespace GameCore.GameScene_Island.LevelItems.InteractItems
{
    public class CuttableZoneInteractItem : InteractItem
    {
        private CuttableItem _cuttableItem => GetComponentInParent<CuttableItem>();
        
        public override int priority { get; } = 4;
        
        private CuttableZone _cuttableZoneCached;

        public CuttableZone cuttableZone
        {
            get
            {
                if (_cuttableZoneCached == null) _cuttableZoneCached = GetComponent<CuttableZone>();
                return _cuttableZoneCached;
            }
        }
        
        private List<InteractorCharacterModel> _interactors = new List<InteractorCharacterModel>();

        private void Awake()
        {
            _cuttableItem.onNotEnabled.On(() =>
            {
                DOVirtual.DelayedCall(0.5f, ExitAllInteractors, false).SetLink(gameObject);
            });
        }

        public override void Interact(InteractorCharacterModel interactorModel)
        {
            if (CheckIsAbleToCut())
            {
                cuttableZone.OnEnterZone(interactorModel);
            }
        }

        public override void OnEnter(InteractorCharacterModel interactorModel)
        {
            base.OnEnter(interactorModel);
            
            if (!_interactors.Contains(interactorModel))
            {
                _interactors.Add(interactorModel);
            }
        }
        
        public override void OnExit(InteractorCharacterModel interactorModel)
        {
            base.OnEnter(interactorModel);

            if (_interactors.Contains(interactorModel))
            {
                _interactors.Remove(interactorModel);
                cuttableZone.OnExitZone(interactorModel);
            }
        }

        private bool CheckIsAbleToCut()
        {
            return _cuttableItem.interactItem.enabled;
        }

        private void ExitAllInteractors()
        {
            foreach (var interactor in _interactors)
            {
                cuttableZone.OnExitZone(interactor);
            }
        }

        private void EnterAllInteractors()
        {
            foreach (var interactor in _interactors)
            {
                cuttableZone.OnEnterZone(interactor);
            }
        }
    }
}