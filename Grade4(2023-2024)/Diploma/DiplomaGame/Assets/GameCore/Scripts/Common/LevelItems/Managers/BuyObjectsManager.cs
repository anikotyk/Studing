using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.Common.Controllers;
using GameCore.Common.Misc;
using GameCore.GameScene.LevelItems.Platforms;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Game.Managers;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Managers
{
    public abstract class BuyObjectsManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public SceneLoader sceneLoader { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [InjectOptional, UsedImplicitly] public ICheatPanelGUI cheatPanelGUI { get; }
        [InjectOptional, UsedImplicitly] public TargetCameraOnObjectController targetCameraOnObjectController { get; }

        [SerializeField] private bool _showBuySpotAtStart = true;
        [SerializeField] private bool _showEachNextBuySpot = true;
        
        private List<BuyObject> _buyObjectsCached;
        public List<BuyObject> buyObjects
        {
            get
            {
                if (_buyObjectsCached == null) _buyObjectsCached = GetComponentsInChildren<BuyObject>(true).ToList();
                return _buyObjectsCached;
            }
        }

        public virtual BuyObject currentBuyObject => activeBuyObjectIndexSaveProperty.value < buyObjects.Count ? buyObjects[activeBuyObjectIndexSaveProperty.value] : null;
        
        protected TheSaveProperty<int> _activeBuyObjectIndexSaveProperty;
        public TheSaveProperty<int> activeBuyObjectIndexSaveProperty => _activeBuyObjectIndexSaveProperty;
        protected TheSaveProperty<bool> _watchedCutsceneSaveProperty;

        private bool _isCheatsBuyEnabled = false;
        public bool isCheatsBuyEnabled => _isCheatsBuyEnabled;
        
        public readonly TheSignal<BuyObject> onSetInBuyModeBuyObject = new();

        private void Awake()
        {
            GetSaves();
        }

        public override void Construct()
        {
            base.Construct();
            initializeInOrderController.Add(Initialize, 1000);
            
            if (cheatPanelGUI != null)
            {
                cheatPanelGUI.onDraw.On(OnDraw).Priority(100);
            }
        }

        protected virtual void GetSaves()
        {
            _activeBuyObjectIndexSaveProperty = new(CommStr.ActiveBuyObjectIndex_Raft, linkToDispose: gameObject);
            _watchedCutsceneSaveProperty = new(CommStr.WatchedCutsceneArrive_Raft);
        }
        
        protected virtual void Initialize()
        {
            for (var i = 0; i < buyObjects.Count; i++)
            {
                var buyObject = buyObjects[i];
                if (_activeBuyObjectIndexSaveProperty.value == i)
                {
                    if (i != 0 || _watchedCutsceneSaveProperty.value)
                    {
                        buyObject.SetInBuyModeInternal(true);
                        onSetInBuyModeBuyObject.Dispatch(buyObject);
                    }
                    else
                    {
                        buyObject.DeactivateInternal();
                    }
                }
                else if (_activeBuyObjectIndexSaveProperty.value > i)
                {
                    buyObject.ActivateInternal();
                }
                else
                {
                    buyObject.DeactivateInternal();
                }
            }

            if (_showBuySpotAtStart && _activeBuyObjectIndexSaveProperty.value > 1 && _activeBuyObjectIndexSaveProperty.value < buyObjects.Count)
            {
                ShowCurrentBuyObject();
            }
        }

        public void SetInBuyModeCurrentObject()
        {
            if (currentBuyObject)
            {
                currentBuyObject.SetInBuyMode();
                onSetInBuyModeBuyObject.Dispatch(currentBuyObject);
            }
        }

        private void ShowNextBuyObject()
        {
            if(!currentBuyObject || currentBuyObject.GetComponent<IgnoreAtShowNextBuyObject>()) return;
            
            int prevIndex = _activeBuyObjectIndexSaveProperty.value - 1;
            if(prevIndex < 0) return;
            var prev = buyObjects[prevIndex];
            
            var windowShowable = prev.GetComponent<IWindowShowable>();
            if (windowShowable != null)
            {
                windowShowable.onWindowClosed.Once(ShowCurrentBuyObject);
            }
            else
            {
                ShowCurrentBuyObject();
            }
        }

        public void ShowCurrentBuyObject()
        {
            if(!currentBuyObject) return;
            var target = currentBuyObject.GetComponentInChildren<DonateResourcesPlatform>(true);
            if (target)
            {
                targetCameraOnObjectController.ShowObject(target.transform);
            }
        }

        public void ActivateNext()
        {
            _activeBuyObjectIndexSaveProperty.value++;
            SetInBuyModeCurrentObject();

            if (_showEachNextBuySpot)
            {
                ShowNextBuyObject();
            }

            UpdateAstarPath();
            
        }
        
        private void UpdateAstarPath()
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                AstarPath.active.Scan();
            }, false).SetLink(gameObject);
        }
        
        private void OnDraw(float width, float height)
        {
            GUILayout.BeginVertical("Box", GUILayout.Width(width));
            GUILayout.Label("BuyCheats");
            Cheats();
            GUILayout.EndVertical();
        }

        protected virtual void Cheats()
        {
            GUILayout.BeginHorizontal();
            if (!isCheatsBuyEnabled)
            {
                if (GUILayout.Button("Enable buy cheats"))
                {
                    _isCheatsBuyEnabled = true;
                }
            }
            else
            {
                if (GUILayout.Button("Disable buy cheats"))
                {
                    _isCheatsBuyEnabled = false;
                }
            }

            if (!_isSceneCompleted)
            {
                if (GUILayout.Button("Complete this scene"))
                {
                    CompleteScene();
                }
            }

            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            if (_showEachNextBuySpot)
            {
                if (GUILayout.Button("Turn off cam targeting"))
                {
                    _showEachNextBuySpot = false;
                }
            }
            else
            {
                if (GUILayout.Button("Turn on cam targeting"))
                {
                    _showEachNextBuySpot = true;
                }
            }
            GUILayout.EndHorizontal();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (cheatPanelGUI != null)
            {
                cheatPanelGUI.onDraw.Off(OnDraw);
            }
        }

        private bool _isSceneCompleted = false;

        protected virtual void CompleteScene()
        {
            _isSceneCompleted = true;
            _activeBuyObjectIndexSaveProperty.value = buyObjects.Count;
            sceneLoader.Restart();
        }
    }
}