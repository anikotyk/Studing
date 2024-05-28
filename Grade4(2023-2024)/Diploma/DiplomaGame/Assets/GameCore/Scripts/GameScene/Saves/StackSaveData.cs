using System.Collections.Generic;
using System.Linq;
using GameBasicsCore.Game.SaveProperties.SaveData.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Products;
using GameBasicsSDK.GameBasicsCore.Plugins.EasySave3.GameBasics;

namespace GameCore.GameScene.Saves
{
    public class StackSaveData : ES3SavePropertyData<StackSaveData.InternalSaveData>
    {
        public override string key { get; }

        public StackSaveData(string key)
        {
            this.key = key;
        }
        
        [ES3Alias("StackSaveData")]
        public class InternalSaveData : ES3SaveData<InternalSaveData>
        {
            [ES3Serializable] private List<string> _products = new List<string>();

            public void SetProducts(List<ProductView> productsToSet)
            {
                _products = productsToSet.Select(view => view.id).ToList();
                DispatchChange();
            }
            
            public List<string> GetProductsIds()
            {
                return _products;
            }
        }
    }
}