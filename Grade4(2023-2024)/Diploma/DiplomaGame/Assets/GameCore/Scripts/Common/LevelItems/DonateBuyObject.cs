using DG.Tweening;
using GameCore.GameScene.LevelItems.Platforms;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using UnityEngine;

namespace GameCore.Common.LevelItems
{
    public class DonateBuyObject : BuyObject
    {
        [SerializeField] private GameObject _showGameObject;
        [SerializeField] private GameObject _scaleGameObject;
        [SerializeField] private InteractItem _interactItemToEnable;
        [SerializeField] private GameObject[] _objectsActivateAfterScale;
        [SerializeField] private GameObject _hideGameObject;
        [SerializeField] private BuyPlatform _buyPlatform;
        public BuyPlatform buyPlatform => _buyPlatform;
        [SerializeField] private ParticleSystem _vfx;
        
        [SerializeField] private Ease _scaleEase = Ease.OutBack;


        public override void ActivateInternal()
        {
            base.ActivateInternal();
            
            _showGameObject.gameObject.SetActive(true);
            if (_hideGameObject != null)
            {
                _hideGameObject.gameObject.SetActive(false);
            }
            
            _buyPlatform.DeactivateInternal();
        }
        
        public override void DeactivateInternal()
        {
            base.DeactivateInternal();
            
            _showGameObject.gameObject.SetActive(false);
            _buyPlatform.DeactivateInternal();
        }
        
        public override void SetInBuyModeInternal(bool isToUseSaves = false)
        {
            base.SetInBuyModeInternal(isToUseSaves);
            
            _showGameObject.gameObject.SetActive(false);
            _buyPlatform.ActivateInternal(isToUseSaves);
        }
        
        public override void SetInBuyMode()
        {
            base.SetInBuyMode();
            
            _buyPlatform.Activate();
            onSetInBuyMode.Dispatch();
        }
        
        public override void Buy()
        {
            if (isBought) return;
            
            if (_hideGameObject != null)
            {
                _hideGameObject.SetActive(false);
            }
           
            _showGameObject.SetActive(true);
            if (_scaleGameObject != null)
            {
                if (_interactItemToEnable != null)
                {
                    _interactItemToEnable.enabled = false;
                }

                foreach (var obj in _objectsActivateAfterScale)
                {
                    obj.SetActive(false);
                }
                Vector3 scale = _scaleGameObject.transform.localScale;
                _scaleGameObject.transform.localScale = Vector3.one *0.01f;
                _scaleGameObject.transform.DOScale(scale, 0.5f).SetEase(_scaleEase).OnComplete(() =>
                {
                    if (_interactItemToEnable != null)
                    {
                        _interactItemToEnable.enabled = true;
                    }
                    
                    foreach (var obj in _objectsActivateAfterScale)
                    {
                        obj.SetActive(true);
                    }
                }).SetLink(gameObject);
            }
            
            _vfx.gameObject.SetActive(true);
            _vfx.Play();
            DOVirtual.DelayedCall(0.5f, () =>
            {
                _vfx.gameObject.SetActive(false);
            },false).SetLink(gameObject);
            
            //_buyPlatform.Deactivate();
            
            base.Buy();
        }
    }
}

