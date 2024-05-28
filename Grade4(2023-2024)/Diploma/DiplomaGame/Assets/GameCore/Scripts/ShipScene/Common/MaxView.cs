using GameBasicsCore.Game.Views;
using GameBasicsSDK.Modules.IdleArcade.Misc;
using UnityEngine;

namespace GameCore.ShipScene.Common
{
    public class MaxView : CoreView
    {
        public void Max(Vector3 position)
        {
            hub.Get<IASgnl.Misc.Max>().Dispatch(position);
        }

        public void Enough(Vector3 position)
        {
            hub.Get<IASgnl.Misc.Enough>().Dispatch(position);
        }
    }
}