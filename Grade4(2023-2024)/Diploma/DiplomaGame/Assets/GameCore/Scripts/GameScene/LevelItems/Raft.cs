using DG.Tweening;
using GameCore.Common.LevelItems.Managers;
using JetBrains.Annotations;
using GameBasicsCore.Game.Controllers;
using GameBasicsCore.Game.Core;
using UnityEngine;
using Zenject;

namespace GameCore.GameScene.LevelItems
{
    public class Raft : InjCoreMonoBehaviour
    {
        [Inject, UsedImplicitly] public BuyObjectsManager buyObjectsManager { get; }
        [Inject, UsedImplicitly] public InitializeInOrderController initializeInOrderController { get; }
        
        private Bounds _bounds;
        public Bounds bounds => _bounds;

        public override void Construct()
        {
            base.Construct();

            buyObjectsManager.activeBuyObjectIndexSaveProperty.onChange.On((_)=>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    RecalculateBounds();
                }, false).SetLink(gameObject);
            });
            
            initializeInOrderController.Add(Initialize, 5000);
        }
        
        private void Initialize()
        {
            RecalculateBounds();
        }
        
        public void RecalculateBounds()
        {
            _bounds = GetBounds();
        }

        public bool IsInsideTheRaft(Vector3 position)
        {
            return bounds.Contains(position);
        }
        
        private Bounds GetBounds()
        {
            return GetBounds(gameObject);
        }
        
        private static Bounds GetBounds(GameObject obj)
        {
            Bounds bounds = new Bounds();

            BoxCollider[] boxColliders = obj.GetComponentsInChildren<BoxCollider>();

            if (boxColliders.Length > 0)
            {
                foreach (var collider in boxColliders)
                {
                    if (collider.gameObject.activeSelf)
                    {
                        bounds = collider.bounds;
                        break;
                    }
                }
                
                foreach (var collider in boxColliders)
                {
                    if (collider.gameObject.activeSelf)
                    {
                        bounds.Encapsulate(collider.bounds);
                    }
                }
            }
            
            return bounds;
        }
    }
}