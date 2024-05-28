using GameCore.Common.LevelItems;
using UnityEngine;

namespace GameCore.GameScene_Island.LevelItems.Animal
{
    public class PointAnimalPath : MonoBehaviour
    {
        [SerializeField] private int _floor;
        public int floor => _floor;
        [SerializeField] private bool _isPrivatePath;
        public bool isPrivatePath => _isPrivatePath;
       
        private BuyObject _buyObject => GetComponentInParent<BuyObject>(true);

        public bool IsAvailable()
        {
            return !_buyObject || _buyObject.isBought;
        }
    }
}
