using System.Linq;
using GameCore.Common.Misc;
using GameCore.GameScene_Island.LevelItems.Items;
using GameCore.GameScene.LevelItems.Platforms;
using GameBasicsCore.Game.SaveProperties.Api;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Platforms
{
    public class WaterFilterIslandBuyPlatform : BuyPlatform
    {
        [SerializeField] private OldFilter _oldFilter;
        [SerializeField] private ProductDataConfig _oldFilterConfig;
        
        private TheSaveProperty<bool> _isOldFilterTaken;

        protected override void Validate(bool isToUseSaves = false)
        {
            base.Validate(isToUseSaves);
            
            _isOldFilterTaken = new(CommStr.OldFilterTaken);
            if (_isOldFilterTaken.value)
            {
                var priceProd = _priceList.FirstOrDefault(item => item.config.id == _oldFilterConfig.id);
                priceProd.SetCount(1);
            }
            else
            {
                _oldFilter.onTaken.Once(() =>
                {
                    var priceProd = _priceList.FirstOrDefault(item => item.config.id == _oldFilterConfig.id);
                    priceProd.SetCount(1);
                    AfterGetSomeProduct();
                });
            }
        }
    }
}

