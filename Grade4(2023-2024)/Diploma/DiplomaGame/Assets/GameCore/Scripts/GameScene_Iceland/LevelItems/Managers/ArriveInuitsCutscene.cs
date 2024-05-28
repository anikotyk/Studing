using System.Collections;
using GameCore.Common.Controllers;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameCore.Common.UI;
using GameCore.CutScenes.Animations.Items;
using GameCore.GameScene.LevelItems.Platforms;
using JetBrains.Annotations;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class ArriveInuitsCutscene : Cutscene
    {
        [Inject, UsedImplicitly] public TargetCameraOnObjectController targetCameraOnObjectController { get; }
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        
        [SerializeField] private BuyObject _buyObjectActivate;
        [SerializeField] private GameObject _cutsceneGO;
        [SerializeField] private CutSceneMoveAnimationItem _cutSceneMoveAnimationItem;
        [SerializeField] private GameObject _realInuits;
        [SerializeField] private Sprite _inuitSprite;
        [SerializeField] private string _inuitText;
        [SerializeField] private bool _isToShowNextBuyObject = true;
        
        protected override bool deactivateMainCharacter => false;
        
        public readonly TheSignal onFakeInuitsDeactivated = new();

        public override void Construct()
        {
            base.Construct();
            _buyObjectActivate.onBuy.Once(StartCutscene);
        }

        protected override IEnumerator CutsceneCoroutine()
        {
            if(_realInuits) _realInuits.SetActive(false);
            yield return new WaitForSeconds(1f);
            _cutsceneGO.SetActive(true);
           
            yield return new WaitForSeconds(2f);
            topTextHint.ShowHint("Somebody is coming!");

            _cutSceneMoveAnimationItem.onCutsceneCompleted.Once(() =>
            {
                windowFactory.Create<CharacterReplicaDialog>(CommStr.CharacterReplicaDialog, (window) =>
                {
                    window.Initialize(_inuitSprite, _inuitText);
                    window.Show();
                    window.onHideStart.Once((_) =>
                    {
                        if(_realInuits) _realInuits.SetActive(true);
                        _cutsceneGO.SetActive(false);
                        onFakeInuitsDeactivated.Dispatch();
                    });
                    window.onHideComplete.Once((_) =>
                    {
                        if (_isToShowNextBuyObject)
                        {
                            buyObjectsManager.ShowCurrentBuyObject();
                        }
                    });
                });
                OnEndScene();
            });
        }
    }
}