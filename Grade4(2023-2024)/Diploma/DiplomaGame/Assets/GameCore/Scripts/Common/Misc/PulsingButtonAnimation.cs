using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace GameCore.Common.Misc
{
    [RequireComponent(typeof(Button))]
    public class PulsingButtonAnimation : MonoBehaviour
    {
        [SerializeField] private bool _useFirstChildAsSecondParam = true;
        [SerializeField, HideIf(nameof(IsUseFirstChildAsSecondParam))] private Transform _secondParam;

        private Button _buttonCached;
        public Button button => _buttonCached ??= GetComponent<Button>();
        
        private Transform second => _useFirstChildAsSecondParam ? transform.GetChild(0) : _secondParam;

        private void Start()
        {
            button.onClick.AddListener(Stop);
        }

        private void OnEnable()
        {
            Play();
        }

        private void OnDisable()
        {
            Stop();
        }

        public void Play()
        {
            var tweens = GCAnimations.PlayPulseScaleButtonAndLabelAnimation(transform, second);
            tweens.tweenButton.SetId(this).SetLink(gameObject);
            tweens.tweenLabel.SetId(this).SetLink(gameObject);
        }

        public void Stop()
        {
            DOTween.Kill(this);
            transform.localScale = Vector3.one;
            second.localScale = Vector3.one;
        }

        public bool IsUseFirstChildAsSecondParam() => _useFirstChildAsSecondParam;
    }
}