using JetBrains.Annotations;
using GameBasicsCore.Game.API.Analytics;
using GameBasicsCore.Game.Views.UI.Windows.Dialogs;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.Common.UI
{
    public class SurveyDialog : UIDialogWindow
    {
        [SerializeField] private Button _btnSurvey;
        [InjectOptional, UsedImplicitly] public IDesignAnalytics designAnalytics { get; }

        protected override void Init()
        {
            base.Init();

            onCloseClick.On(() =>
            {
                designAnalytics?.NewDesignEvent("Survey:Close");
                Debug.Log("Survey:Close");
            });
            
            _btnSurvey.onClick.AddListener(() =>
            {
                Hide();
                Application.OpenURL("https://homagames.typeform.com/to/srDxpGqo");
                designAnalytics?.NewDesignEvent("Survey:Click");
                Debug.Log("Survey:Click");
                
                buttonUiClickSfxPlayer?.Play();
            });
        }
    }
}