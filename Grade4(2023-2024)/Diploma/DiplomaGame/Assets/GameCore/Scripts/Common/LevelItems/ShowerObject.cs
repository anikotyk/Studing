using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.Common.LevelItems.Items;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.Common.LevelItems
{
    public class ShowerObject : InjCoreMonoBehaviour
    {
        [SerializeField] private GameObject _visibleObj;
        [SerializeField] private float _speed;
        [SerializeField] private Transform[] _showerObjects;

        private Coroutine _showerCoroutine;
        private Tween _moveTween;
        private Tween _scaleTween;
        
        public void StartShower()
        {
            List<Transform> listToShower = new List<Transform>();
            foreach (var obj in _showerObjects)
            {
                if (obj.gameObject.activeInHierarchy)
                {
                    listToShower.Add(obj);
                }
            }

            if (listToShower.Count > 0)
            {
                if(_showerCoroutine!=null) StopCoroutine(_showerCoroutine);
                if(_moveTween!=null) _moveTween.Kill();
                _showerCoroutine = StartCoroutine(StartShower(listToShower));
            }
        }
        
        private IEnumerator StartShower(List<Transform> objects)
        {
            _visibleObj.SetActive(true);
            if(_scaleTween!=null) _scaleTween.Kill();
            _visibleObj.transform.localScale = Vector3.one * 0.01f;
            _scaleTween = _visibleObj.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
            
            yield return new WaitForSeconds(0.5f);

            int loops = 1;
            for (int i = 0; i < loops; i++)
            {
                foreach (var obj in objects)
                {
                    Vector3 pos = obj.position;
                    if (obj.GetComponent<IWatering>()!=null)
                    {
                        pos = obj.GetComponent<IWatering>().pointWatering.position;
                    }
                    
                    pos.y = transform.position.y;
                    float time = Vector3.Distance(transform.position, pos) / _speed;
                    _moveTween = transform.DOMove(pos, time).SetLink(gameObject);
                    yield return new WaitForSeconds(time);
                    if(obj.GetComponent<IWatering>()!=null) obj.GetComponent<IWatering>().OnWatered();
                    if (time <= 0.01f)
                    {
                        yield return new WaitForSeconds(1f);
                        if(obj.GetComponent<IWatering>()!=null) obj.GetComponent<IWatering>().OnWatered();
                    }
                }
            }

            _scaleTween = _visibleObj.transform.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _visibleObj.SetActive(false);
            }).SetLink(gameObject);
        }
    }
}