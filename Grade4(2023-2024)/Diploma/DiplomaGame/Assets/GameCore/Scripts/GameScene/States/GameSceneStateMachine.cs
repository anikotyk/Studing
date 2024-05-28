using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.LevelSystem;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.States;
using GameBasicsCore.Game.Views.UI.Windows;
using GameBasicsSDK.Modules.IdleArcade.Misc;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.States
{
    public class GameSceneStateMachine : StateMachine
        <GameSceneStateMachine.Start, 
        GameSceneStateMachine.Play,
        GameSceneStateMachine.MiniGame,
        GameSceneStateMachine.End>
    {
        public class Start : State
        {
            public override string id => NCStr.Start;
        }
        public class Play : State
        {
            public override string id => NCStr.Play;
        }
        public class MiniGame : State
        {
            public override string id => IAStr.MiniGame;
        }
        public class End : State
        {
            public override string id => NCStr.End;
        }

        [Inject, UsedImplicitly] public Level level { get; }

        public override void Initialize()
        {
            Debug.Log("GameSceneStateMachine Initialize");
            
            // When hiding start window (main menu) switch state to Play.
            hub.Get<NCSgnl.IUIWindowSignals.HideStart>().On(window =>
            {
                if (window is ILevelStartWindow)
                {
                    level.Play();
                    SetState<Play>();
                }
            }).OffWhen(() => state is Play);

            hub.Get<NCSgnl.ReviveSignals.Apply>().On(SetState<Play>);
            
            hub.Get<IASgnl.MiniGameSignals.Play>().On(_ => SetState<MiniGame>());
            hub.Get<IASgnl.MiniGameSignals.Stop>().On(_ => SetState<Play>());
            
            // When showing level end window switch state to Play.
            hub.Get<NCSgnl.IUIWindowSignals.ShowStart>().On(window =>
            {
                if (window is ILevelEndWindow) SetState<End>();
                if (window is ReviveWindow) SetState<End>();
            });//.OffWhen(() => state is End);
            
            base.Initialize();
        }
    }
}