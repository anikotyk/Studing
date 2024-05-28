using System.Collections.Generic;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.Common.LevelItems.Character.Modules
{
    public class InteractionCharacterModule : InteractorCharacterModule
    {
        private List<CharacterModule> _lockers = new List<CharacterModule>();
        
        public readonly TheSignal onInteractionsAvailable = new();
        public readonly TheSignal onInteractionsLocked = new();
        
        public bool CanInteract()
        {
            CheckIsStillInteracting();
            
            return _lockers.Count <= 0;
        }

        private void CheckIsStillInteracting()
        {
            foreach(var locker in _lockers)
            {
                if (!locker.isNowRunning)
                {
                    OnInteractionEnd(locker);
                }
            }
        }

        public void OnInteractionStart(CharacterModule provider)
        { 
            if(_lockers.Contains(provider)) return;
            _lockers.Add(provider);
            onInteractionsLocked.Dispatch();
        }
        
        public void OnInteractionEnd(CharacterModule provider)
        {
            if (_lockers.Contains(provider))
            {
                _lockers.Remove(provider);
            }
            if(CanInteract()) onInteractionsAvailable.Dispatch();
        }
    }
}