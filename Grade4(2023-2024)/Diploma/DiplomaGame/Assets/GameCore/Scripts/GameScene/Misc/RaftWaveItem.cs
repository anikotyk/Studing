using DG.Tweening;
using GameBasicsCore.Game.Core;
using UnityEngine;

namespace GameCore.GameScene.Misc
{
    public class RaftWaveItem : MonoBehaviour
    {
        [SerializeField] private int _lineIndex;

        public void SetWaving()
        {
            transform.DOMoveY(0.1f, 1.5f).SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Yoyo).SetDelay(_lineIndex * 0.75f, false).SetLink(gameObject);
        }
    }
}