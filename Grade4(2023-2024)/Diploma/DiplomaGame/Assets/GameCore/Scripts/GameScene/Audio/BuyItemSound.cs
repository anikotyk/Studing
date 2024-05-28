using UnityEngine;

namespace GameCore.GameScene.Audio
{
    public class BuyItemSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _sound;
        public AudioSource sound => _sound;
    }
}
