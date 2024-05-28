using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore.GameScene.Misc
{
    public class RaftWave : MonoBehaviour
    {
        private List<RaftWaveItem> items;

        private void Start()
        {
            items = GameObject.FindObjectsOfType<RaftWaveItem>(true).ToList();
            
            foreach (var item in items)
            {
               item.SetWaving();
            }
        }
    }
}