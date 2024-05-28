using System;
using System.Collections.Generic;
using GameCore.Common.LevelItems;
using GameCore.Common.LevelItems.Character.CharacterClothing;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class CharactersClothingManager : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        [SerializeField] private List<CharacterClotheActivationData> _charactersClothesActivationsDataList;

        public override void Construct()
        {
            base.Construct();
            initializeInOrderController.Add(ValidateClothes, 4000);
        }

        private void ValidateClothes()
        {
            foreach (var characterClotheSave in _charactersClothesActivationsDataList)
            {
                if (characterClotheSave.buyObjectActivate.isBought)
                {
                    characterClotheSave.clothesManager.SetCharacterDressing(characterClotheSave.dressing);
                }
                else
                {
                    characterClotheSave.buyObjectActivate.onBuy.Once(() =>
                    {
                        characterClotheSave.clothesManager.SetCharacterDressing(characterClotheSave.dressing);
                        //TODO: effects
                    });
                }
            }
        }
    }

    [Serializable]
    public struct CharacterClotheActivationData
    {
        public CharacterModelClothesManager clothesManager;
        public CharacterDressing dressing;
        public BuyObject buyObjectActivate;
    }
}