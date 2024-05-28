using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene_Island.LevelItems.Items;
using JetBrains.Annotations;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Items
{
    public class Scythe : MonoBehaviour
    {
        [Inject, UsedImplicitly] private InteractorCharacterModel interactor { get; }
        
        private Collider _sensor;
        private bool _isTurnedOn;
        
        private void Awake()
        {
            _sensor = GetComponent<Collider>();
            _sensor.OnTriggerEnterAsObservable()
                .Select(item =>
                {
                    CuttableItem res = item.GetComponent<CuttableItem>();
                    if (res == null)
                    {
                        if (item.transform.parent != null)
                        {
                            return item.transform.GetComponentInParent<CuttableItem>();
                        }
                    }
                    return res;
                })
                .Where(item => item != null)
                .Subscribe(item =>
                {
                    if (_isTurnedOn && _sensor.gameObject.activeInHierarchy && item.interactItem.enabled && interactor.view.GetModule<CuttingModule>() && interactor.view.GetModule<CuttingModule>().CanCutNow(item.spawnProductConfig))
                    {
                        item.Cut(interactor is MainCharacterModel);
                    }
                });
        }

        public void TurnOn()
        {
            _isTurnedOn = true;
        }
        public void TurnOff()
        {
            _isTurnedOn = false;
        }
    }
}