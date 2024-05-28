using System.Collections.Generic;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Tools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.Api;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Interaction.Items;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Rooms.Participants;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.Helper
{
    public class HelperInteractionValidateController : ControllerInternal, IInteractionValidator
    {
        [Inject, UsedImplicitly] public HelperView helper { get; }

        private RoomParticipant _helperRoomParticipant;

        public override void Construct()
        {
            _helperRoomParticipant = helper.GetComponent<RoomParticipant>();
            if (_helperRoomParticipant == null)
            {
                Debug.LogError("Helper's RoomParticipant can't be null.");
            }
        }

        public void ValidateItems(List<InteractItem> interactionItems)
        {
            interactionItems.RemoveBy(item =>
            {
                var roomParticipant = item.GetComponent<RoomParticipant>();
                if (roomParticipant == null) return false;
                if (_helperRoomParticipant.room == null && roomParticipant.room == null)
                {
                    return false;
                }
                return roomParticipant.room != _helperRoomParticipant.room;
            });
        }
    }
}