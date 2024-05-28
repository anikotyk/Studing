using System.Linq;
using JetBrains.Annotations;
using GameBasicsCore.Game.API;
using GameBasicsCore.Game.Enums;
using GameBasicsCore.Game.Misc;
using GameBasicsCore.Game.Settings.GameCore;
using GameBasicsCore.Game.States;
using GameBasicsCore.Game.Tools.EventCatcherService;
using Zenject;

namespace GameCore.GameScene.States
{
    public class GameSceneStateHandler : SceneStateHandler
    {
        [Inject, UsedImplicitly] public EventCatcher eventCatcher { get; set; }
        [Inject, UsedImplicitly] public ILevelStartWindow levelStartWindow { get; set; }
        
        public override void Construct()
        {
            base.Construct();
            
            EventCatcher.EventSignal eventSignal;
            switch (GameCoreSettings.def.controls.interactToPlay)
            {
                case InteractToPlay.Down: eventSignal = eventCatcher.onDown; break;
                case InteractToPlay.Drag: eventSignal = eventCatcher.onDrag; break;
                case InteractToPlay.Up: eventSignal = eventCatcher.onUp; break;
                default: eventSignal = eventCatcher.onUp; break;
            }
            
            eventSignal.Once(_ => levelStartWindow.Hide());
            
            hub.Get<NCSgnl.StateMachineSignals.Switch>().On(state =>
            {
                _switchables
                    .OrderByDescending(s => s.priority)
                    .ToList()
                    .ForEach(s =>
                    {
                        if (s.HasActivationState(state.id))
                        {
                            s.Activate();
                        }
                        else if (s.HasDeactivationState(state.id))
                        {
                            s.Deactivate();
                        }
                    });
            });
            /*var listener = new StateListener(hub, GCStr.Play);
            listener.EnableListen(true);
            listener.onStateSet.On(on =>
            {
                if (on)
                {
                    joystickController.Activate();
                }
                else
                {
                    joystickController.Deactivate();
                }
            });*/
        }
    }
}
