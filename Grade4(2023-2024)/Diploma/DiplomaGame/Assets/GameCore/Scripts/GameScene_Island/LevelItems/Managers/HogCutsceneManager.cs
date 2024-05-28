using System.Collections;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene_Island.LevelItems.Tutorials;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Managers
{
    public class HogCutsceneManager : Cutscene
    {
        [SerializeField] private HogTrapTutorial _hogTrapTutorial;
        [SerializeField] private BuyObject _buyObjectActivate;
        [SerializeField] private GameObject _hogCam;
        [SerializeField] private WateringObjectsGroup _wateringObjectsGroup;
        [SerializeField] private GameObject _trapCam;
        
        protected override bool deactivateMainCharacter => false;
        
        public override void Construct()
        {
            base.Construct();

            _buyObjectActivate.onBuy.Once(StartCutscene);
        }
        
        protected override IEnumerator CutsceneCoroutine()
        {
            _wateringObjectsGroup.SetWateredAndRespawned();
            yield return new WaitForSeconds(0.5f);
            _hogCam.SetActive(true);
            yield return new WaitForSeconds(6f);
            _hogCam.SetActive(false); 
            _trapCam.SetActive(true);
            _hogTrapTutorial.StartTutorial();
            yield return new WaitForSeconds(1.5f);
            _trapCam.SetActive(false);
            yield return new WaitForSeconds(1f);
            OnEndScene();
        }
    }
}