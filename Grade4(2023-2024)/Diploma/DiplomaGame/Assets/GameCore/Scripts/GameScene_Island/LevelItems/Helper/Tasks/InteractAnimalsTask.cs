using System.Collections;
using System.Collections.Generic;
using GameCore.Common.LevelItems.Character.Modules;
using GameCore.GameScene_Island.LevelItems.Animal;
using GameCore.GameScene_Island.LevelItems.Character.Modules;
using GameCore.GameScene.Helper;
using GameCore.GameScene.Helper.Modules;
using GameCore.GameScene.Helper.Tasks;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Helper.Tasks
{
    public class InteractAnimalsTask : HelperTask
    {
        private HelperView _view;
        private InteractorCharacterProductsCarrier _carrier;

        private AnimalProductingView[] _animals;
        private List<AnimalProductingView> _usedAnimals = new List<AnimalProductingView>();
        
        public void Initialize(HelperView view, AnimalProductingView[] animals)
        {
            _view = view;
            _animals = animals;
        }
        
        protected override IEnumerator RunInternal()
        {
            while (true)
            {
                var animal = GetAnimal();
                if(animal == null) break;
                _usedAnimals.Add(animal);
                yield return null;

                _view.taskModule.MoveTo(animal.name, animal.productionModule.interactPoint.position);

                yield return null;

                float timer = 0f;
                while (!_view.taskModule.aiPath.reachedDestination && animal.productionModule.interactItem.enabled && timer < maxTimeMoveToPoint)
                {
                    yield return null;
                    timer += Time.deltaTime;
                }
               
                _view.locomotionMovingModule.StopMovement();
                
                if (_view.taskModule.aiPath.reachedDestination && animal.productionModule.interactItem.enabled)
                {
                    _view.GetModule<CuttingModule>().EndCuttingFully();
                    yield return new WaitForSeconds(2f);
                }
            }
        }

        protected override void StopInternal()
        {
            base.StopInternal();
            _usedAnimals.Clear();
        }

        private AnimalProductingView GetAnimal()
        {
            foreach (var animal in _animals)
            {
                if (animal.productionModule && animal.productionModule.interactItem.enabled && !_usedAnimals.Contains(animal)) return animal;
            }

            return null;
        }
    }
}