using GameBasicsSignals;
using UnityEngine;

namespace GameCore.Common.Misc
{
    public class FootstepsHandler : MonoBehaviour
    {
        public readonly TheSignal onLeftStep = new();
        public readonly TheSignal onRightStep = new();
        
        public void LeftStep()
        {
            onLeftStep.Dispatch();  
        }
    
        public void RightStep()
        {
            onRightStep.Dispatch();  
        }  
    }
}