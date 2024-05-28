using UnityEngine;

#pragma warning disable 0649
namespace GameCore.Common.Misc
{
    /// <summary>
    /// Add this to main Camera.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class UICameraRef : MonoBehaviour
    {
        private Camera _cameraCached;
        public new Camera camera => _cameraCached ??= GetComponent<Camera>();

        public static implicit operator Camera(UICameraRef r) => r.camera;
    }
}