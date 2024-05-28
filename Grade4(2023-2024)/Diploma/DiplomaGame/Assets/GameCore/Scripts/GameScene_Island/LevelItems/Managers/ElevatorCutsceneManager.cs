using System.Collections;
using DG.Tweening;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Managers
{
    public class ElevatorCutsceneManager : Cutscene
    {
        [Inject, UsedImplicitly] public ShipCraft shipCraft { get; }

        [SerializeField] private BuyObject _buyObjectActivate;
        [SerializeField] private GameObject _boy;
        [SerializeField] private Transform _elevatorPlatform;
        [SerializeField] private Elevator _elevator;
        [SerializeField] private Transform _pointMoveElevator;
        [SerializeField] private Transform _pointMoveBoy;
        [SerializeField] private Transform _pointStartShip;
        [SerializeField] private Transform _pointMoveShip;
        
        [SerializeField] private AnimatorParameterApplier _walkAnimBoy;
        [SerializeField] private AnimatorParameterApplier _stopWalkAnimBoy;
        
        [SerializeField] private GameObject _startCam;
        [SerializeField] private GameObject _shipCam;
        
        protected override bool deactivateMainCharacter => false;

        public override void Construct()
        {
            base.Construct();

            _buyObjectActivate.onBuy.Once(StartCutscene);
        }
        
        protected override IEnumerator CutsceneCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            
            AstarPath.active.Scan();
            
            yield return new WaitForSeconds(0.5f);
            
            SwitchToCamera(_startCam);
            
            mainCharacterView.gameObject.SetActive(false);
            shipCraft.shipStageRuined.SetActive(true);
            shipCraft.shipStageRuined.transform.position = _pointStartShip.transform.position;

            yield return null;
            _boy.transform.localScale = Vector3.one * 0.01f;
            _boy.gameObject.SetActive(true);
            _boy.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
            
            yield return new WaitForSeconds(1.5f);
            _boy.transform.SetParent(_elevatorPlatform);
            _elevatorPlatform.DOMoveY(_pointMoveElevator.position.y, 2.5f).SetLink(gameObject);
            yield return new WaitForSeconds(2.5f);
            _boy.transform.DOLookAt(_pointMoveBoy.position, 0.5f).SetLink(gameObject);
            yield return new WaitForSeconds(0.5f);
            _walkAnimBoy.Apply();
            yield return new WaitForSeconds(0.1f);
            _boy.transform.DOMove(_pointMoveBoy.position, 1.5f).SetEase(Ease.Linear).SetLink(gameObject);
           
            windowFactory.Create<ChapterCompleteDialog>("ChapterComplete", window =>
            {
                window.SetChapterNumber(2);
                window.Show();
            });
            yield return new WaitForSeconds(1.4f);
            _stopWalkAnimBoy.Apply();
            _boy.transform.DORotate(_pointMoveBoy.rotation.eulerAngles, 1f).SetLink(gameObject);
            yield return new WaitForSeconds(1f);
            SwitchToCamera(_shipCam);
            yield return new WaitForSeconds(2.1f);
            shipCraft.shipStageRuined.transform.DOMove(_pointMoveShip.position, 1f).SetEase(Ease.InOutBack);
            yield return new WaitForSeconds(1f);
            topTextHint.ShowHint("Hopeful ship", fadeValue : 0.9f);
            yield return new WaitForSeconds(1f);
            mainCharacterView.transform.position = _boy.transform.position;
            mainCharacterView.transform.rotation = _boy.transform.rotation;
                    
            TurnOffCurrentCamera();
            mainCharacterView.gameObject.SetActive(true);
            _elevator.OnTopNow();
            _boy.gameObject.SetActive(false);
            _boy.transform.SetParent(transform);
            OnEndScene();
            _elevator.TurnOn();
        }
    }
}