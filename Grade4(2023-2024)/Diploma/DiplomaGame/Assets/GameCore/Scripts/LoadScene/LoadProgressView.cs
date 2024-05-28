using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.LoadScene
{
    public class LoadProgressView : MonoBehaviour
    {
        [SerializeField] private Image _progress;

        private CanvasGroup _groupCached;
        public CanvasGroup group
        {
            get
            {
                if (_groupCached == null) _groupCached = GetComponent<CanvasGroup>();
                return _groupCached;
            }
        }

        private void Awake()
        {
            _progress.fillAmount = 0;
        }

        public void SetProgress(float value)
        {
            _progress.DOFillAmount(value, 0.15f).SetEase(Ease.Linear).SetLink(gameObject);
        }
    }
}