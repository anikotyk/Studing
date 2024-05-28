using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Managers;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.GameScene.Misc
{
    public class RaftsCount : InjCoreMonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private Image _progressBar;
        [SerializeField] private BuyObject _firstObject;
        [SerializeField] private BuyObject _deactivateObject;
        
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }

        public override void Construct()
        {
            base.Construct();
            
            initializeInOrderController.Add(Initialize, 2000);
        }

        private void Initialize()
        {
            if ((_firstObject == null || _firstObject.isBought) && (_deactivateObject == null || !_deactivateObject.isBought)) 
            {
                gameObject.SetActive(true);
                SetCount(buyObjectsManager.activeBuyObjectIndexSaveProperty.value);
                if (_deactivateObject !=null)
                {
                    _deactivateObject.onBuy.Once(() =>
                    {
                        gameObject.SetActive(false);
                    });
                }
            }
            else
            {
                gameObject.SetActive(false);
                if (_firstObject !=null && !_firstObject.isBought)
                {
                    _firstObject.onBuy.Once(() =>
                    {
                        gameObject.SetActive(true);
                        SetCount(buyObjectsManager.activeBuyObjectIndexSaveProperty.value);
                    });
                }
            }
           
            buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.On(SetCount);
        }

        private void SetCount(int index)
        {
            int count = index;
            int targetCount = buyObjectsManager.buyObjects.Count;
            
            if (_firstObject != null)
            {
                count = index - buyObjectsManager.buyObjects.IndexOf(_firstObject);
            }
            if (_deactivateObject != null && _firstObject != null)
            {
                targetCount = buyObjectsManager.buyObjects.IndexOf(_deactivateObject) - buyObjectsManager.buyObjects.IndexOf(_firstObject);
            }else if (_deactivateObject != null)
            {
                targetCount = buyObjectsManager.buyObjects.IndexOf(_deactivateObject);
            }else if (_firstObject != null)
            {
                targetCount =  buyObjectsManager.buyObjects.Count - buyObjectsManager.buyObjects.IndexOf(_firstObject);
            }

            if (count > targetCount)
            {
                count = targetCount;
            }
            _countText.text = count + "/"+ targetCount;
            _progressBar.fillAmount = count * 1f / targetCount;
        }
    }
}