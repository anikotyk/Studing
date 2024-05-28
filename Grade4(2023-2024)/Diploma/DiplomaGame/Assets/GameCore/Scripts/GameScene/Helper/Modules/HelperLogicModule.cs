using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using GameCore.GameScene.Helper.Tasks;
using GameCore.GameScene.Settings;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Blockers;
using GameBasicsCore.Game.Views.UI.Windows.Menus;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.Helper.Modules
{
    public class HelperLogicModule : InjCoreMonoBehaviour
    {
        [SerializeField] private AnimatorParameterApplier _restAnim;
        [SerializeField] private AnimatorParameterApplier _endRestAnim;
        [SerializeField] protected Transform _defaultPoint;
        public virtual Transform defaultPoint => _defaultPoint;
        [SerializeField] private GameObject _visible;
        
        [Inject, UsedImplicitly] public HelperController helperController { get; }
        [Inject, UsedImplicitly] public List<VirtualCameraLink> virtualCameraLinks { get; }
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        [Inject, UsedImplicitly] public GameplayUIMenuWindow gameplayUIMenuWindow { get; }
        [Inject, UsedImplicitly] public MenuBlockOverlay menuBlockOverlay { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        
        private CinemachineVirtualCamera _helperAppearVCamCached;
        public CinemachineVirtualCamera helperAppearVCam
        {
            get
            {
                if (_helperAppearVCamCached == null)
                {
                    _helperAppearVCamCached =  virtualCameraLinks.First(v => v.id == appearVCamConfig).virtualCamera;
                }
                return _helperAppearVCamCached;
            }
        }

        public virtual string appearVCamConfig => GameplaySettings.def.helperAppearVCamIdConfig.id;
        
        
        private HelperView _viewCached;
        public HelperView view => _viewCached ??= GetComponentInParent<HelperView>(true);
        
        
        private InteractorCharacterProductsCarrier _carrierCached;
        public InteractorCharacterProductsCarrier carrier {
            get
            {
                if (_carrierCached == null) _carrierCached = view.GetComponentInChildren<InteractorCharacterProductsCarrier>(true);
                return _carrierCached;
            }
        }
        
        protected HelperTasksQueueGroup _tasksGroup;
        private RestHelperTask _restTask;
        protected RestHelperTask restTask => _restTask;
        private SellHelperTask _sellTask;
        protected SellHelperTask sellTask => _sellTask;
        
        private bool _isApplicationClose = false;

        private bool _isTurnedOn = false;
        public bool isTurnedOn => _isTurnedOn;

        public override void Construct()
        {
            initializeInOrderController.Add(Initialize, 10000);
        }

        protected virtual void Initialize()
        {
            
        }

        protected void ValidateHelper()
        {
            AstarPath.active.Scan();
            TurnOnHelper();
            SetUpTasks();
        }

        protected virtual void SetUpTasks()
        {
            SetUpSellTask();
            SetUpRestTask();
        }
        
        private void SetUpSellTask()
        {
            _sellTask = new SellHelperTask();
            _sellTask.Initialize(view, carrier, view.sellStorageModule.moveStoragePoint, view.sellStorageModule.storageCarrier);
        }
        
        private void SetUpRestTask()
        {
            _restTask = new RestHelperTask();
            _restTask.Initialize(view, _restAnim, _endRestAnim, _sellTask);
        }
        
        public void TurnOnHelper()
        {
            _isTurnedOn = true;
            view.taskModule.aiPath.canMove = true;
            if (_tasksGroup != null)
            {
                _tasksGroup.EnableTaskGroup();
            }
        }
        
        public void TurnOffHelper()
        {
            _isTurnedOn = false; 
            if (carrier.count > 0)
            {
                carrier.Clear();
            }
            if (_tasksGroup != null)
            {
                _tasksGroup.DisableTaskGroup();
            }
            view.taskModule.aiPath.canMove = false;
            view.locomotionMovingModule.StopMovement();
        }

        public void ResetHelper()
        {
            if (carrier.count > 0)
            {
                carrier.Clear();
            }
            view.locomotionMovingModule.StopMovement();
            view.transform.position = defaultPoint.position;
            _tasksGroup.RestartTask();
        }
        
        private void OnApplicationQuit()
        {
            _isApplicationClose = true;
        }

        private void OnDisable()
        {
            if (_isApplicationClose) return;
            if (_tasksGroup != null)
            {
                _tasksGroup.DisableTaskGroup();
            }
        }

        private void OnEnable()
        {
            if (_tasksGroup != null)
            {
                _tasksGroup.EnableTaskGroup();
            }
        }

        public void HideVisible()
        {
            _visible.gameObject.SetActive(false);
        }
        
        public void ShowVisible()
        {
            _visible.gameObject.SetActive(true);
        }
    }
}