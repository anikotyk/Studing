using DG.Tweening;
using UnityEngine;

namespace GameCore.Common.Misc
{
    public class PulseAnim : MonoBehaviour
    {
        void Start()
        {
            transform.DOScale(Vector3.one * 1.1f, 0.75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        }
    }
}