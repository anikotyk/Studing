using MPUIKIT;
using GameBasicsCore.Game.Views.UI.PopUps;
using UnityEngine;

namespace GameCore.GameScene_Island.UI
{
    public class ProgressPopUp : ImagePopUpView
    {
        [SerializeField] private MPImageBasic _progess;

        public void SetProgress(float progess)
        {
            _progess.fillAmount = progess;
        }
    }
}