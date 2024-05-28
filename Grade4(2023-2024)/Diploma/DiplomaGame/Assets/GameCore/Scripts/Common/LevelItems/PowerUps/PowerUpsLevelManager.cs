using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCore.Common.LevelItems.PowerUps
{
    public class PowerUpsLevelManager : MonoBehaviour
    {
        [SerializeField] private float _spawnRadius = 1f;

        [SerializeField] private BuyObject _capacitySuperPowerAvailable;
        public BuyObject capacitySuperPowerAvailable => _capacitySuperPowerAvailable;
        [SerializeField] private BuyObject _speedSuperPowerAvailable;
        public BuyObject speedSuperPowerAvailable => _speedSuperPowerAvailable;
        [SerializeField] private BuyObject _productionSuperPowerAvailable;
        public BuyObject productionSuperPowerAvailable => _productionSuperPowerAvailable;
        [SerializeField] private BuyObject _hittingSuperPowerAvailable;
        public BuyObject hittingSuperPowerAvailable => _hittingSuperPowerAvailable;
        [SerializeField] private BuyObject _stackOfCashAvailable;
        public BuyObject stackOfCashAvailable => _stackOfCashAvailable;
        
        private List<PowerUpPoint> _points;

        private void Awake()
        {
            _points = GameObject.FindObjectsOfType<PowerUpPoint>(true).ToList();
        }

        private PowerUpPoint GetRandomPoint()
        {
            var availablePoints = _points.FindAll(item => item.IsAvailable());
            if (availablePoints.Count <= 0) return null;
            int index = Random.Range(0, availablePoints.Count);
            return availablePoints[index];
        }

        public Vector3 GetPowerUpPosition()
        {
            Vector3 pos = transform.position;
            PowerUpPoint point = GetRandomPoint();
            if (point != null) pos = point.transform.position;
            return RandomizePosition(pos);
        }

        private Vector3 RandomizePosition(Vector3 pos)
        {
            float xOffset = Random.Range(-_spawnRadius, _spawnRadius);
            float zOffset = Random.Range(-_spawnRadius, _spawnRadius);
            
            pos += new Vector3(xOffset, 0, zOffset);
            return pos;
        }
    }
}
