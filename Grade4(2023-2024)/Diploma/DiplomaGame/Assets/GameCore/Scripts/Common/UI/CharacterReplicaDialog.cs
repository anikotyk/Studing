using DG.Tweening;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Common.UI
{
    public class CharacterReplicaDialog : UIDialogWindow
    {
        [SerializeField] private Transform _characterMoveContainer;
        [SerializeField] private CanvasGroup _characterCanvasGroup;
        [SerializeField] private Transform _textContainer;
        [SerializeField] private CanvasGroup _textCanvasGroup;
        [SerializeField] private Image _characterImage;
        [SerializeField] private TextMeshProUGUI _replicaText;

        protected override void Init()
        {
            base.Init();
            _characterMoveContainer.localPosition = Vector3.right * -100f;
            _textContainer.localPosition = Vector3.right * -25f + Vector3.up * -25f;
        }
        
        public void Initialize(Sprite characterSprite, string text)
        {
            _replicaText.gameObject.SetActive(false);
            _characterImage.sprite = characterSprite;
            _replicaText.text = text;
        }

        protected override void ShowContent(Sequence seq)
        {
            seq.Append(_characterMoveContainer.DOLocalMove(Vector3.zero, 0.25f).SetId(this).SetLink(gameObject));
            seq.Join(_textContainer.DOLocalMove(Vector3.zero, 0.25f).SetId(this).SetLink(gameObject));
            seq.Join(_characterCanvasGroup.DOFade(1f, 0.25f).From(0f).SetId(this).SetLink(gameObject));
            seq.Join(_textCanvasGroup.DOFade(1f, 0.25f).From(0f).SetId(this).SetLink(gameObject));
            seq.AppendCallback(() => _replicaText.gameObject.SetActive(true));
            //seq.Append(dialog.transform.DOPunchScale(Vector3.one * 0.05f, 0.25f, 0, 0).SetId(this).SetLink(gameObject));
        }

        protected override void HideContent(Sequence seq)
        {
            seq.Join(_characterMoveContainer.DOLocalMoveX(-50f, 0.25f).SetId(this).SetLink(gameObject));
            seq.Join(_textContainer.DOLocalMove(new Vector3(-25f, -25f), 0.25f).SetId(this).SetLink(gameObject));
            seq.Join(_characterCanvasGroup.DOFade(0, 0.25f).SetId(this).SetLink(gameObject));
            seq.Join(_textCanvasGroup.DOFade(0, 0.25f).SetId(this).SetLink(gameObject));
        }
    }
}