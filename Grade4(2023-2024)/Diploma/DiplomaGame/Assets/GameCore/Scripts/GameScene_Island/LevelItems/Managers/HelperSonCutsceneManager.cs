using System.Collections;
using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using GameCore.GameScene.Helper;
using JetBrains.Annotations;
using GameBasicsCore.Game.Misc;
using GameBasicsSignals;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Island.LevelItems.Managers
{
    public class HelperSonCutsceneManager : Cutscene
    {
        [Inject, UsedImplicitly] public HelperView helperView { get; }

        [SerializeField] private GameObject _boy;
        [SerializeField] private GameObject _girl;
        [SerializeField] private GameObject _hut;
        [SerializeField] private GameObject _realHut;
        
        [SerializeField] private AnimatorParameterApplier _walkAnimBoy;
        [SerializeField] private AnimatorParameterApplier _walkAnimGirl;
        
        [SerializeField] private GameObject _houseCam;
        [SerializeField] private GameObject _houseFarCam;
        
        [SerializeField] private GameObject _vfxHouse;
        [SerializeField] private ParticleSystem _vfxExplosion;
        
        [SerializeField] private Transform _insideHousePointBoy;
        [SerializeField] private Transform _insideHousePointGirl;
        [SerializeField] private Transform _pointGirl;
        [SerializeField] private Animator _girlAnimator;
        
        [SerializeField] private AudioSource[] _audiosToTurnOff;
        [SerializeField] private AudioSource _kissSound;
        [SerializeField] private AudioSource _loveMusic;
        
        public readonly TheSignal onCutsceneEnded = new();
        
        protected override bool deactivateMainCharacter => false;

        protected override IEnumerator CutsceneCoroutine()
        {
            _girlAnimator.enabled = false;
            
            yield return new WaitForSeconds(0.5f);
            
            SwitchToCamera(_houseCam);
            
            mainCharacterView.gameObject.SetActive(false);
            helperView.logicModule.HideVisible();
            foreach (var audio in _audiosToTurnOff)
            {
                audio.Stop();
            }
            
            _boy.gameObject.SetActive(true);
            _girl.gameObject.SetActive(true);
            _hut.gameObject.SetActive(true);
            _realHut.gameObject.SetActive(false);

            yield return new WaitForSeconds(1f);
            _girlAnimator.enabled = true;
            yield return new WaitForSeconds(1.5f);
            _kissSound.Play();
            yield return new WaitForSeconds(1f);
            _girl.transform.DOMove(_pointGirl.position, 0.5f).SetLink(gameObject);
            _girl.transform.DORotate(_pointGirl.rotation.eulerAngles, 0.5f).SetLink(gameObject);
            yield return new WaitForSeconds(0.5f);
            
            topTextHint.ShowHint("Lovely house", fadeValue : 0.9f);
            
            _walkAnimBoy.Apply();
            _walkAnimGirl.Apply();

            _boy.transform.DOMove(_insideHousePointBoy.position, 5f).SetLink(gameObject);
            _girl.transform.DOMove(_insideHousePointGirl.position, 5f).SetLink(gameObject);
            
            yield return new WaitForSeconds(2f);
            SwitchToCamera(_houseFarCam);
            _loveMusic.Play();
            yield return new WaitForSeconds(2f);
            _hut.transform.DOShakeScale(3.9f, 0.05f, 7, fadeOut:false).SetLink(gameObject);
            _vfxHouse.SetActive(true);
            yield return new WaitForSeconds(4f);
            _vfxExplosion.Play();
            _hut.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 1).SetEase(Ease.InOutBack).SetLink(gameObject);
            foreach (var vfx in _vfxHouse.GetComponentsInChildren<ParticleSystem>())
            {
                vfx.Stop();
            }
            yield return new WaitForSeconds(1f);
            
            TurnOffCurrentCamera();
            
            _vfxHouse.SetActive(false);
            
            mainCharacterView.gameObject.SetActive(true);
            helperView.logicModule.ShowVisible();

            _boy.gameObject.SetActive(false);
            _girl.gameObject.SetActive(false);
            _hut.gameObject.SetActive(false);
            _realHut.gameObject.SetActive(true);
            
            OnEndScene();

            _loveMusic.DOFade(0, 0.5f).SetLink(gameObject).OnComplete(() =>
            {
                _loveMusic.Stop();
            });
            
            foreach (var audio in _audiosToTurnOff)
            {
                audio.Play();
            }
            
            onCutsceneEnded.Dispatch();
        }
    }
}