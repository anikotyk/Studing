using System.Collections;
using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameBasicsCore.Game.Misc;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Managers
{
    public class HelperTribeManCutsceneManager : Cutscene
    {
        [SerializeField] private GameObject _tribeMan;
        [SerializeField] private GameObject _mainCharacter;
        [SerializeField] private GameObject _tribeManFeedPlace;
        [SerializeField] private GameObject _vfxHungry;
        
        [SerializeField] private GameObject _tribeManRunCam;
        [SerializeField] private GameObject _tribeManCam;
        [SerializeField] private GameObject _feedingCam;
        
        [SerializeField] private ParticleSystem _vfxSpear;
        
        [SerializeField] private AnimatorParameterApplier _tribeManRun;
        [SerializeField] private AnimatorParameterApplier _tribeManFall;
        [SerializeField] private AnimatorParameterApplier _tribeManStandUp;
        
        [SerializeField] private AnimatorParameterApplier _hitCharAnim;
        
        [SerializeField] private Transform _runPoint;
        [SerializeField] private Transform _fallPoint;
        
        protected override bool deactivateMainCharacter => false;
        
        public readonly TheSignal onCutsceneEnded = new();

        protected override IEnumerator CutsceneCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            
            _mainCharacter.gameObject.SetActive(true);
            
            SwitchToCamera(_tribeManCam);
            
            _tribeMan.gameObject.SetActive(true);
            _vfxHungry.SetActive(true);
            
            yield return new WaitForSeconds(2f);
            topTextHint.ShowHint("Hungry person", fadeValue : 0.9f);
            
            yield return new WaitForSeconds(2f);
            
            SwitchToCamera(_tribeManRunCam);
            
            yield return new WaitForSeconds(1.5f);
            topTextHint.ShowHint("You look yummy!", fadeValue : 0.9f);
            _tribeManRun.Apply();
            _vfxHungry.gameObject.SetActive(false);
            _tribeMan.transform.DOMove(_runPoint.position, 1.5f).SetEase(Ease.Linear).SetLink(gameObject);
            _tribeMan.transform.DORotate(_runPoint.rotation.eulerAngles, 0.2f).SetLink(gameObject);
            yield return new WaitForSeconds(1.2f);
            _hitCharAnim.Apply();
            yield return new WaitForSeconds(0.3f);
            _vfxSpear.Play();
            _tribeManFall.Apply();
            _tribeMan.transform.DOJump(_fallPoint.position, 1, 1, 1f).SetLink(gameObject);
            _tribeMan.transform.DORotate(_fallPoint.rotation.eulerAngles, 0.2f).SetLink(gameObject);
            yield return new WaitForSeconds(0.5f);
            _vfxHungry.gameObject.SetActive(true);
            SwitchToCamera(_tribeManCam);
            _tribeManStandUp.Apply();
            _tribeManFeedPlace.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            SwitchToCamera(_feedingCam);
            topTextHint.ShowHint("Steak = work!", fadeValue : 0.9f);
            yield return new WaitForSeconds(3f);
            
            TurnOffCurrentCamera();
            _mainCharacter.gameObject.SetActive(false);
            
            OnEndScene();
            
            yield return new WaitForSeconds(1f);
            
            _tribeMan.gameObject.SetActive(false);

            onCutsceneEnded.Dispatch();
        }
    }
}