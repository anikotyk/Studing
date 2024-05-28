using System.Collections;
using DG.Tweening;
using GameCore.Common.Misc;
using GameCore.GameScene_Iceland.LevelItems.Animals.KillerWhale;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Misc;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Items
{
    public class SwimHunter : MonoBehaviour
    {
        [Inject, UsedImplicitly] public VfxStack vfxStack { get; }
        
        [SerializeField] private AnimatorParameterApplier _throwSpearAnim;
        [SerializeField] private AnimatorParameterApplier _idleAnim;
        [SerializeField] private GameObject _spear;
        [SerializeField] private ParticleSystem _vfxHit;
        [SerializeField] private AudioSource _hitSound;

        private float _spearSpeed = 20f;
        private float _distanceAttack = 10f;
        
        private Tween _attackTween;
        private Coroutine _attackCoroutine;
        private GameObject _spearCopy;
        private Tween _rotateTween;
        private Tween _startAttackTween;
        
        public void StartAttack(WhaleKillerView whaleKillerView, float delay = 0)
        {
            _rotateTween = DOVirtual.DelayedCall(5f, () => { }, false).OnUpdate(() =>
            {
                transform.LookAt(whaleKillerView.transform.position, Vector3.up);
            }).SetLoops(-1, LoopType.Incremental).SetLink(gameObject);
            _startAttackTween = DOVirtual.DelayedCall(delay, () =>
            {
                _attackCoroutine = StartCoroutine(AttackCoroutine(whaleKillerView));
            }, false).SetLink(gameObject);
        }

        private IEnumerator AttackCoroutine(WhaleKillerView whaleKillerView)
        {
            while (true)
            {
                if (Vector3.Distance(transform.position, whaleKillerView.transform.position) > _distanceAttack)
                {
                    yield return new WaitForSeconds(1f);
                    continue;
                }
                _spear.SetActive(true);
                _throwSpearAnim.Apply();
                yield return new WaitForSeconds(0.3f);
                _spearCopy = Instantiate(_spear);
                _spearCopy.transform.position = _spear.transform.position;
                _spearCopy.transform.localScale = Vector3.one;
                _spearCopy.gameObject.SetActive(true);
                _spear.SetActive(false);
                float time = Vector3.Distance(_spearCopy.transform.position, whaleKillerView.transform.position) / _spearSpeed;
                _spearCopy.transform.DOMove(whaleKillerView.transform.position, time).SetEase(Ease.Linear).OnUpdate(() =>
                {
                    _spearCopy.transform.LookAt(whaleKillerView.transform.position, Vector3.up);
                }).SetLink(gameObject);
                yield return new WaitForSeconds(time/2f);
                _vfxHit.Play();
                _hitSound.Play();
                yield return new WaitForSeconds(time - time/1.5f);
                vfxStack.Spawn(CommStr.BloodExplosionSpiky, _spearCopy.transform.position);
                _spearCopy.gameObject.SetActive(false);
                Hit(whaleKillerView);
            }
        }

        private void Hit(WhaleKillerView whaleKillerView)
        {
            if (whaleKillerView.getDamageModule.isEnabled)
            {
                whaleKillerView.getDamageModule.OnHit();
            }
        }
        
        public void EndAttack()
        {
            if(_startAttackTween!=null) _startAttackTween.Kill();
            if(_attackTween!=null) _attackTween.Kill();
            if(_rotateTween!=null) _rotateTween.Kill();
            if(_attackCoroutine != null) StopCoroutine(_attackCoroutine);
            
            _spear.gameObject.SetActive(false);
            if (_spearCopy != null)
            {
                _spearCopy.gameObject.SetActive(false);
            }
           
            _idleAnim.Apply();
            transform.localRotation = Quaternion.Euler(new Vector3(0, -90f, 0));
        }
    }
}