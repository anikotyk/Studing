using GameCore.Common.Misc;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.Common.LevelItems.Items.HittableItems
{
    public interface IHittable
    {
        void OnHit(float multipler = 1);
        bool isEnabled { get; }
        CharacterTools.HittingToolType toolType { get; }
        Transform view { get; }
        BoxCollider colliderInteract { get; }
        TheSignal onTurnOff { get; }
        TheSignal onTurnOn { get; }
        Vector3 helperPosition { get; }
        float canHitAngle { get; }
    }
}