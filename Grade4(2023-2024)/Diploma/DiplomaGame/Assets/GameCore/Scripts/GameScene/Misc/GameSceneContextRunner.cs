using GameBasicsCore.Game.Contexts;
using GameBasicsCore.Game.LevelSystem;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.Misc
{
    [RequireComponent(typeof(SceneContext))]
    public class GameSceneContextRunner : MonoBehaviour
    {
        private void Start()
        {
            PreProjectContext.instance.Initialize();

            var sceneContext = GetComponent<SceneContext>();
            LevelManager.instance.StartScene(sceneContext);
        }
        
    }

}
