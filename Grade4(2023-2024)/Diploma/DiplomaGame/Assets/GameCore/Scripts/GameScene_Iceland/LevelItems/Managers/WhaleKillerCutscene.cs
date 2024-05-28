using System.Collections;
using Cinemachine;
using GameCore.Common.LevelItems.Managers;
using GameBasicsCore.Game.Misc;
using UnityEngine;

namespace GameCore.GameScene_Iceland.LevelItems.Managers
{
    public class WhaleKillerCutscene : Cutscene
    {
        [SerializeField] private CinemachineVirtualCamera _whaleKillerCam;
        [SerializeField] private AnimatorParameterApplier _attackAnim;
        [SerializeField] private AudioSource _roarSound;
        
        protected override bool deactivateMainCharacter => false;
        protected override bool deactivateGameplayUIWindow => false;
        protected override bool deactivateTutorialArrow => false;
        protected override bool activateMenuBlockOverlay => false;
        
        protected override IEnumerator CutsceneCoroutine()
        {
            SwitchToCamera(_whaleKillerCam.gameObject);
            yield return new WaitForSeconds(1.75f);
            _attackAnim.Apply();   
            yield return new WaitForSeconds(0.1f);
            _roarSound.Play();
            yield return new WaitForSeconds(1.15f);
            TurnOffCurrentCamera();
            yield return new WaitForSeconds(2f);
            OnEndScene();
        }
    }
}