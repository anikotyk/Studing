using System.Collections;
using GameCore.Common.LevelItems.Managers;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using UnityEngine;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class InuitsGoSwimHuntingCutscene : Cutscene
    {
        [SerializeField] private GameObject _inuitsHouseCam;
        [SerializeField] private AnimatorParameterApplier _inuit1Anim;
        [SerializeField] private AnimatorParameterApplier _inuit2Anim;
        [SerializeField] private AudioSource[] _sounds;
        
        protected override bool deactivateMainCharacter => false;
        protected override bool deactivateGameplayUIWindow => false;
        protected override bool deactivateTutorialArrow => false;
        protected override bool activateMenuBlockOverlay => false;
        
        protected override IEnumerator CutsceneCoroutine()
        {
            SwitchToCamera(_inuitsHouseCam);
            yield return new WaitForSeconds(2f);
            topTextHint.ShowHint("Ready to hunting");
            yield return new WaitForSeconds(0.5f);
            _inuit1Anim.Apply();
            _inuit2Anim.Apply();
            yield return new WaitForSeconds(0.5f);
            foreach (var sound in _sounds)
            {
                sound.Play();
            }
            yield return new WaitForSeconds(1.5f);
            UIDialogWindow blackScreen = null;
            windowFactory.Create<UIDialogWindow>("BlackScreen", (window) =>
            {
                blackScreen = window;
                blackScreen.Show();
                blackScreen.onShowComplete.Once((_) =>
                {
                    TurnOffCurrentCamera();
                    OnEndScene();
                    blackScreen.Hide();
                });
            });
        }
    }
}