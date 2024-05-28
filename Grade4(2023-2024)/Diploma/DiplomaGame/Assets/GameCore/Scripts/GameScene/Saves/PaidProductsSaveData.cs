using System.Collections.Generic;
using GameBasicsCore.Game.SaveProperties.SaveData.Base;
using GameBasicsSDK.GameBasicsCore.Plugins.EasySave3.GameBasics;

namespace GameCore.GameScene.Saves
{
    public class PaidProductsSaveData : ES3SavePropertyData<PaidProductsSaveData.InternalSaveData>
    {
        public override string key => "PaidProducts";

        [ES3Alias("PaidProducts")]
        public class InternalSaveData : ES3SaveData<InternalSaveData>
        {
            [ES3Serializable] private Dictionary<string, int> products = new Dictionary<string, int>();

            public void AddPaidProductCount(string id, int cnt)
            {
                if (!products.ContainsKey(id))
                {
                    products.Add(id, 0);
                }

                products[id] += cnt;
                
                DispatchChange();
            }

            public int GetProductCount(string id)
            {
                if (!products.ContainsKey(id))
                {
                    return 0;
                }

                return products[id];
            }

            public void Clear()
            {
                products.Clear();
                DispatchChange();
            }
        }
    }
}