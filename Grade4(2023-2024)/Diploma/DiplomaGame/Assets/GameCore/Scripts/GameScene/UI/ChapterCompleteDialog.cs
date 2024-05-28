using DG.Tweening;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using TMPro;
using UnityEngine;

namespace GameCore.GameScene.UI
{
    public class ChapterCompleteDialog : UIDialogWindow
    {
        [SerializeField] private TextMeshProUGUI _text;

        public override bool playSoundOnShowAndHide => false;
        
        private Tween _tween;

        public void SetChapterNumber(int number)
        {
            _text.text = "Chapter " + number + "\nCOMPLETED";
            DOVirtual.DelayedCall(0.5f, () =>
            {
                _text.transform.DOLocalMoveY(300f, 3f).SetRelative(true).OnComplete(() =>
                {
                    Hide();
                }).SetLink(gameObject);
            }, false).SetLink(gameObject);
        }
    }
}