using UnityEngine;

namespace GameCore.GameScene.LevelItems
{
    public class Sea : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        public Collider seaCollider => _collider;
    }
}