using Cinemachine;
using DG.Tweening;
using GameCore.Common.Controllers;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.GameScene_Iceland.LevelItems.Managers;
using GameCore.GameScene.DataConfigs;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Factories;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Animals.KillerWhale.Modules
{
    public class WhaleKillerDeathModule : InjCoreMonoBehaviour
    {
        [InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }
        [Inject, UsedImplicitly] public SwimHuntingManager swimHuntingManager { get; }
        [Inject, UsedImplicitly] public VfxStack vfxStack { get; }
        [Inject, UsedImplicitly] public UIWindowFactory windowFactory { get; }
        [Inject, UsedImplicitly] public SpawnProductsManager spawnProductsManager { get; }
        [Inject, UsedImplicitly] public ProductionController productionController { get; }

        [SerializeField] private AnimatorParameterApplier _dieAnim;
        [SerializeField] private CinemachineVirtualCamera _dieCam;
        [SerializeField] private ProductDataConfig _spawnProductConfig;
        [SerializeField] private int _spawnProductsCount;
        [SerializeField] private ProductsCarrier _spawnProductsCarrier;
        [SerializeField] private AudioSource _waterJumpSound;
        [SerializeField] private AudioSource _dieSound;
        [SerializeField] private AudioSource _explodeSound;
        [SerializeField] private AudioSource _winSound;
        
        [SerializeField] private string _productName;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Sprite _iconOutline;
        [SerializeField] private ProductPriceDataConfig _priceDC;
        private TheSaveProperty<bool> _isWhaleKillerProductWindowShown;

        private bool _isDied;

        private WhaleKillerView _viewCached;
        public WhaleKillerView view => _viewCached ??= GetComponentInParent<WhaleKillerView>(true);

        public override void Construct()
        {
            base.Construct();
            view.healthAnimalModule.onDied.On(OnDied);
            _isWhaleKillerProductWindowShown =  new(CommStr.WhaleKillerProductWindowShownOnce, linkToDispose: gameObject);
        }
        
        private void OnDied()
        {
            if(_isDied) return;
            _isDied = true;
            
            view.getDamageModule.DisableModule();
            view.attackingModule.StopAttack();
            view.attackingModule.LockStartAttack();
            view.healthAnimalModule.DeactivateHealth();
            
            _dieAnim.Apply();
            _dieSound.Play();
            view.animator.transform.DORotate(new Vector3(0, 90, 0), 0.1f).SetLink(gameObject);
            DOVirtual.DelayedCall(0.25f, () =>
            {
                _dieCam.gameObject.SetActive(true);
            }, false).SetLink(gameObject);
           
            view.transform.DOLocalMoveY(10, 1f).SetRelative(true).SetEase(Ease.Linear).OnComplete(() =>
            {
                float time = Mathf.Abs(view.transform.localPosition.y - 0) / 25f;

                DOVirtual.DelayedCall(time/2, () =>
                {
                    Vector3 vfxPos = view.transform.position;
                    vfxPos.y = view.transform.parent.position.y;
                    vfxStack.Spawn(CommStr.WaterSplash, vfxPos);
                    _waterJumpSound.Play();
                }, false).SetLink(gameObject);
               
                view.transform.DOLocalMoveY(0, time).SetEase(Ease.InOutBack, 0.5f, 0).OnComplete(() =>
                {
                    view.transform.DOScale(Vector3.one * 1.25f, 0.25f).SetDelay(0.25f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        view.transform.DOScale(Vector3.one * 0.01f, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            view.animator.gameObject.SetActive(false);
                        }).SetLink(gameObject);
                        _explodeSound.Play();
                        vfxStack.Spawn(CommStr.BloodExplosion, view.transform.position);
                   
                        DOVirtual.DelayedCall(1f, () =>
                        {
                            windowFactory.Create<UIDialogWindow>("BlackScreen", window =>
                            {
                                window.Show();
                                window.onShowComplete.Once((_) =>
                                {
                                    SpawnProducts();
                                    window.Hide();
                                    view.ResetView();
                                    _dieCam.gameObject.SetActive(false);
                                    swimHuntingManager.StopHunting();
                                    _isDied = false;
                                    _winSound.Play();

                                    if (!_isWhaleKillerProductWindowShown.value)
                                    {
                                        _isWhaleKillerProductWindowShown.value = true;
                                        ShowProductWindow();
                                    }
                                });
                            });
                        }, false).SetLink(gameObject);
                    }).SetLink(gameObject);
                }).SetLink(gameObject);
            }).SetLink(gameObject);
        }

        private void SpawnProducts()
        {
            int spawnCnt = (int)(_spawnProductsCount * productionController.productionMultiplier);
            for (int i = 0; i < spawnCnt; i++)
            {
                ProductView prod = spawnProductsManager.Spawn(_spawnProductConfig, _spawnProductsCarrier.transform.position);
                _spawnProductsCarrier.Add(prod, false);
            }
        }

        private void ShowProductWindow()
        {
            hapticService?.Selection();
            
            windowFactory.Create<UnlockNewProductDialog>("UnlockNewProductDialog", window =>
            {
                window.Initialize(_productName, _priceDC.softCurrencyCount, _icon, _iconOutline);
                window.Show();
            }, false);
        }
    }
}