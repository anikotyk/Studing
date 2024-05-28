using DG.Tweening;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal.Modules
{
    public class SheepProductionModule : AnimalProductionModule
    {
        [SerializeField] private Transform _fur;

        private Vector3 _furScale;
        
        protected override void Awake()
        {
            base.Awake();
            _furScale = _fur.localScale;
        }
        
        protected override void InteractorOnGetProducts(InteractorCharacterModel interactorModel)
        {
            interactorModel.view.GetModule<AnimalInteractModule>().OnSheepInteract();
        }

        protected override void BecomeAvailableSpawn()
        {
            base.BecomeAvailableSpawn();
            
            _fur.gameObject.SetActive(true);
            _fur.localScale = Vector3.zero;
            _fur.DOScale(_furScale, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
        }
        
        protected override void EffectOnGetProduction()
        {
            base.EffectOnGetProduction();

            _fur.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _fur.gameObject.SetActive(false);
            }).SetLink(gameObject);
        }
    }
}