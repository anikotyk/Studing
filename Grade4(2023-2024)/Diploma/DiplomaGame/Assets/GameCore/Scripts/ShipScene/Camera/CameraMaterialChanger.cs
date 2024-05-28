using System.Collections.Generic;
using UnityEngine;

namespace GameCore.ShipScene.Camera
{
    [RequireComponent(typeof(Collider))]
    public class CameraMaterialChanger : MonoBehaviour
    {
        [SerializeField] private List<MaterialData> _changeData;
        
        [System.Serializable]
        public class MaterialData
        {
            public Renderer renderer;
            public Material[] materials;

            public MaterialData(Renderer renderer, Material[] materials)
            {
                this.renderer = renderer;
                this.materials = materials;
            }
        }

        private List<MaterialData> _defaultMaterials = new();

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out CameraCollider _))
                ChangeMaterials();
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent(out CameraCollider _))
                ResetToDefaultMaterials();
        }

        private void GetDefaultMaterials()
        {
            if(_defaultMaterials.Count > 0)
                return;
            foreach (var materialData in _changeData)
            {
                _defaultMaterials.Add(new MaterialData(materialData.renderer, materialData.renderer.sharedMaterials));
            }
        }

        private void ChangeMaterials()
        {
            GetDefaultMaterials();
            foreach (var materialData in _changeData)
                materialData.renderer.sharedMaterials = materialData.materials;
            
        }

        private void ResetToDefaultMaterials()
        {
            GetDefaultMaterials();
            foreach (var materialData in _defaultMaterials)
                materialData.renderer.sharedMaterials = materialData.materials;
        }
    }
}