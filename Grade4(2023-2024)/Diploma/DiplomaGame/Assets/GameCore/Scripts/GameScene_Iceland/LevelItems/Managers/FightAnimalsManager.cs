using System.Collections.Generic;
using DG.Tweening;
using GameCore.Common.LevelItems.Animals.FightAnimal;
using GameCore.GameScene_Island.UI;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Enums;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class FightAnimalsManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public PopUpsController popUpsController { get; }
        
        [SerializeField] private List<FightAnimalView> _animals;
        public List<FightAnimalView> animals => _animals;
        [SerializeField] private float _delayActivateAnimal = 10f;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _popUpPoint;

        private int _animalIndex;
        
        private ProgressPopUp _respawnAnimalPopUp;
        private Tween _scheduleTween;

        public override void Construct()
        {
            base.Construct();
            DeactivateAllAnimals();
            ScheduleActivationNextAnimal();
        }

        public void ActivateAnimal(FightAnimalView animal)
        {
            if(_scheduleTween!=null) _scheduleTween.Kill();
            
            HideRespawnAnimalPopUp();
            DeactivateAllAnimals();
            
            animal.Activate();
            animal.healthAnimalModule.onDied.Off(ScheduleActivationNextAnimal);
            animal.healthAnimalModule.onDied.Once(ScheduleActivationNextAnimal);

            animal.transform.position = _spawnPoint.position;
            animal.transform.rotation = _spawnPoint.rotation;
            animal.transform.localScale = Vector3.one * 0.01f;
            animal.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
        }

        private void ScheduleActivationNextAnimal()
        { 
            var animal = _animals[_animalIndex];
            _animalIndex = (_animalIndex + 1) % _animals.Count;
            SetRespawnAnimalPopUp(animal.icon);
           
            if(_scheduleTween!=null) _scheduleTween.Kill();
            float timer = 0;
            _scheduleTween = DOVirtual.DelayedCall(_delayActivateAnimal, ()=>
            {
                ActivateAnimal(animal);
            }, false).OnUpdate(()=>
            {
                timer += Time.deltaTime;
                _respawnAnimalPopUp.SetProgress(timer/_delayActivateAnimal);
            }).SetLink(gameObject);
        }

        private void DeactivateAllAnimals()
        {
            foreach (var animal in _animals)
            {
                animal.Deactivate();
            }
        }
        
        private void SetRespawnAnimalPopUp(Sprite icon)
        {
            if (_respawnAnimalPopUp == null)
            {
                if(popUpsController == null || popUpsController.containerUnderMenu == null) return;
                _respawnAnimalPopUp = popUpsController.SpawnUnderMenu<ProgressPopUp>("ProgressPopUp");
                _respawnAnimalPopUp.worldSpaceConverter.updateMethod = UpdateMethod.Update;
                _respawnAnimalPopUp.worldSpaceConverter.followWorldObject = _popUpPoint.transform;
            }
            _respawnAnimalPopUp.SetSprite(icon);
            _respawnAnimalPopUp.transform.localScale = Vector3.one;
            _respawnAnimalPopUp.gameObject.SetActive(true);
            _respawnAnimalPopUp.SetProgress(0);
        }

        private void HideRespawnAnimalPopUp()
        {
            _respawnAnimalPopUp.gameObject.SetActive(false);
        }
    }
}