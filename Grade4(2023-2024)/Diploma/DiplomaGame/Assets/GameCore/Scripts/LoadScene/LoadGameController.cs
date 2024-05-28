using System.Collections;
using DG.Tweening;
using GameCore.Common.Misc;
using GameCore.Common.Saves;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.LevelSystem;
using GameBasicsCore.Game.Views.UI.TransitionOverlays;
using GameBasicsCore.Tools.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameCore.LoadScene
{
    public class LoadGameController : CoreController
    {
        [InjectOptional, UsedImplicitly] public ISdkInitializer sdkInitializer { get; }
        [Inject, UsedImplicitly] public LoadProgressView progressView { get; }
        [Inject, UsedImplicitly] public EventSystem eventSystem { get; }
        [Inject, UsedImplicitly] public GameSaveData gameSaveData { get; }
        [Inject, UsedImplicitly] public TransitionOverlayManager transitionOverlayManager { get; set; } 
        [InjectOptional, UsedImplicitly] public DownloadingContentController downloadingContentController { get; }

        public override void Construct()
        {
            transitionOverlayManager.overlay.gameObject.SetActive(false);
            if (sdkInitializer?.initialized == false)
            {
                sdkInitializer.onInitialize.Once(Load);
            }
            else
            {
                Load();
            }
        }
        
        private void Load()
        {
            Debug.Log("LoadGameController: Load");
            
            progressView.StartCoroutine(RunLoad());
        }
        
        private IEnumerator RunLoad()
        {
            Debug.Log("LoadGameController: Run Load");
            // Wait for a little
            yield return new WaitForSeconds(0.1f);
            
            var lastPlayedScene = gameSaveData.value.lastPlayedScene;
            var sceneName = CommStr.GameScene_Raft;
            if (!string.IsNullOrEmpty(lastPlayedScene)) sceneName = lastPlayedScene;
            // Start loading GameScene
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;
            
            while (asyncLoad.progress < 0.9f)
            {
                progressView.SetProgress(asyncLoad.progress);
                yield return null;
            }
            
            // Wait for a little
            yield return new WaitForSeconds(0.1f);

            eventSystem.gameObject.DestroyGameObject();
            
            progressView.SetProgress(0.9f);
            asyncLoad.allowSceneActivation = true;
            while (!asyncLoad.isDone) 
            {
                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            progressView.SetProgress(1f);

            if (downloadingContentController != null)
            {
                var task = downloadingContentController.CheckForUpdates();

                yield return new WaitUntil(() => task.IsCompleted);
            }
            
            while (!LevelManager.initialized) yield return null;
            
            // Wait till LevelManager starts
            while (!LevelManager.instance.started) yield return null;
            
            // Run transition
            //yield return new WaitForSeconds(0.1f);
            
            // SceneContext sceneContext = FindObjectOfType<SceneContext>();
            
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName)); OLD position of activation
            
            // var levelThemeController = sceneContext.Container.Resolve<LevelThemeController>();
            // levelThemeController.theme.ApplyRenderSettings();

            if (progressView.group)
            {
                var tween = progressView.group.DOFade(0f, 0.3f).SetEase(Ease.Linear).SetLink(progressView.group.gameObject);
                yield return tween.WaitForCompletion();
            }
            SceneManager.UnloadSceneAsync(progressView.gameObject.scene);
        }
    }
}