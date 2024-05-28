using System.Collections;
using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.Common.Misc;
using GameBasicsCore.Game.Misc;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Managers
{
    public class FlyCutsceneManager : Cutscene
    {
        [SerializeField] private EventObtainObject _eventObtainObject;
        [SerializeField] private GameObject _containerCharacter;
        [SerializeField] private GameObject _character;
        [SerializeField] private Transform _pointMoveBoy;
        [SerializeField] private GameObject _parachute;
        [SerializeField] private AnimatorParameterApplier _idleAnimBoy;
        [SerializeField] private AnimatorParameterApplier _flyAnimBoy;
        [SerializeField] private AudioSource[] _audiosToStop;
        [SerializeField] private GameObject _startCam;
        
        public override void Construct()
        {
            base.Construct();

            _eventObtainObject.onClosedObtainDialog.Once(StartCutscene);
        }

        protected override IEnumerator CutsceneCoroutine()
        {
            foreach (var audio in _audiosToStop)
            {
                audio.Stop();
            }
            SwitchToCamera(_startCam);
            
            yield return new WaitForSeconds(2f);
            
            _containerCharacter.transform.localScale = Vector3.one * 0.01f;
            _containerCharacter.gameObject.SetActive(true);
            _containerCharacter.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
            
            yield return new WaitForSeconds(0.5f);
            
            topTextHint.ShowHint("First flight", fadeValue : 0.9f);
            _parachute.gameObject.SetActive(true);
            _parachute.transform.localScale = Vector3.zero;
            _parachute.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).SetDelay(0.2f).SetLink(gameObject);
            _containerCharacter.transform.DOMove(_pointMoveBoy.position, 3f).SetEase(Ease.Linear).SetLink(gameObject);
            _containerCharacter.transform.DOLocalRotate(Vector3.forward * 30f, 0.75f).SetEase(Ease.Linear).SetDelay(0.1f).OnComplete(() =>
            {
                _containerCharacter.transform.DOLocalRotate(Vector3.forward * -30f, 1.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    _containerCharacter.transform.DOLocalRotate(Vector3.zero, 0.6f).SetEase(Ease.Linear).SetLink(gameObject);
                }).SetLink(gameObject);
            }).SetLink(gameObject);
            _flyAnimBoy.Apply();
            
            yield return new WaitForSeconds(3f);
            
            _idleAnimBoy.Apply();
            _parachute.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).SetLink(gameObject);
            mainCharacterView.transform.position = _character.transform.position;
            mainCharacterView.transform.rotation = _character.transform.rotation;
            
            TurnOffCurrentCamera();
            
            yield return new WaitForSeconds(1f);
            
            _containerCharacter.gameObject.SetActive(false);
            
            OnEndScene();
            foreach (var audio in _audiosToStop)
            {
                audio.Play();
            }
        }
    }
}