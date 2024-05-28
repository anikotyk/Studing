using System.Collections;
using System.Collections.Generic;
using GameCore.GameScene.LevelItems.Products;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace GameCore.GameScene.Misc
{
    public class Magnet : MonoBehaviour
    {
        [SerializeField] private ProductsCarrier _carrier;
        [SerializeField] private float _speed = 1f;
        private Collider _sensor;

        private List<SellProduct> _currentMovingObjects = new List<SellProduct>();

        private void Awake()
        {
            _sensor = GetComponent<Collider>();
            _sensor.OnTriggerEnterAsObservable()
                .Select(item =>
                {
                    SellProduct res = item.GetComponent<SellProduct>();
                    if (res == null)
                    {
                        if (item.transform.parent != null)
                        {
                            return item.transform.GetComponentInParent<SellProduct>();
                        }
                    }
                    return res;
                })
                .Where(item => item != null)
                .Subscribe(item =>
                {
                    if (_carrier.HasSpace() && _carrier.IsAccepting(item.id) && !_currentMovingObjects.Contains(item)&& !item.isInCarrier && item.interactItem.enabled)
                    {
                        StartCoroutine(MoveCloser(item));
                    }
                });
            
            _sensor.OnTriggerStayAsObservable()
                .Select(item =>
                {
                    SellProduct res = item.GetComponent<SellProduct>();
                    if (res == null)
                    {
                        if (item.transform.parent != null)
                        {
                            return item.transform.GetComponentInParent<SellProduct>();
                        }
                    }
                    return res;
                })
                .Where(item => item != null)
                .Subscribe(item =>
                {
                    if (_carrier.HasSpace() && _carrier.IsAccepting(item.id) && !_currentMovingObjects.Contains(item) && _carrier.enabled && !item.isInCarrier && item.interactItem.enabled)
                    {
                        StartCoroutine(MoveCloser(item));
                    }
                });
        }

        private IEnumerator MoveCloser(SellProduct product)
        {
            _currentMovingObjects.Add(product);
            product.onAddedToCarrier.Once(() =>
            {
                if (_currentMovingObjects.Contains(product))
                {
                    _currentMovingObjects.Remove(product);
                }
            });
            
            bool endedAnims = false;
            bool isMovingForward = false;
            if (product.TryGetComponent<SeaAnimatedProduct>(out SeaAnimatedProduct seaAnimatedProduct))
            {
                if (seaAnimatedProduct.isSeaAnimStarted && product.interactItem.enabled && !product.isInCarrier)
                {
                    seaAnimatedProduct.EndAnims();
                    isMovingForward = seaAnimatedProduct.isMovingForward;
                    endedAnims = true;
                }
            }
            
            product.TurnOffInteractItem();
           
            while (!product.isInCarrier && _carrier.HasSpace() && _carrier.enabled) 
            {
                if (Vector3.Distance(transform.position, product.transform.position) < 0.25f) break;
                product.transform.position += (transform.position - product.transform.position).normalized * _speed * Time.deltaTime;
                yield return null;
            }
            
            product.TurnOnInteractItem();

            if (endedAnims && product.interactItem.enabled && !product.isInCarrier)
            {
                if (product.TryGetComponent<SeaAnimatedProduct>(out SeaAnimatedProduct seaAnimatedProduct2))
                {
                    seaAnimatedProduct2.StartSeaIdleAnim(isMovingForward);
                }
            }
            
            if (_currentMovingObjects.Contains(product))
            {
                _currentMovingObjects.Remove(product);
            }
        }
    }
}