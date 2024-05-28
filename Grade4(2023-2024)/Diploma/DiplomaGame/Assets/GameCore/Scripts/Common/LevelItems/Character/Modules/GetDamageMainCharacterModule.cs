using DG.Tweening;
using GameCore.Common.LevelItems.Controllers;
using GameCore.GameScene.LevelItems.Character.Modules;
using GameCore.GameScene.LevelItems.Products;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Common.LevelItems.Character.Modules
{
    public class GetDamageMainCharacterModule : GetDamageCharacterModule
    {
        [Inject, UsedImplicitly] public ResourcesPopUpsController resourcesPopUpsController { get; }

        [SerializeField] private AudioSource _damageSound;
        
        private int _loseResourcesCnt = 2;
        public override void GetDamage()
        {
            base.GetDamage();
            LoseResources();
            _damageSound.Play();
        }
        
        private void LoseResources()
        {
            for (int i = 0; i < _loseResourcesCnt; i++)
            {
                if (character.GetModule<StackModule>().carrier.count <= 0) break;
                var prod = character.GetModule<StackModule>().carrier.GetOutLast();
                prod.transform.SetParent(null);
                prod.gameObject.SetActive(true);
                if (prod is SellProduct)
                {
                    (prod as SellProduct).TurnOffOutline();
                    (prod as SellProduct).TurnOffInteractItem();
                }

                float range = 0.75f;
                Vector3 pos = character.transform.position + Vector3.up*0.1f + Vector3.forward * Random.Range(-range, range) + Vector3.right * Random.Range(-range, range);
                prod.transform.DOJump(pos, 1.5f, 2, 0.5f).OnComplete(()=>
                {
                    resourcesPopUpsController.SpawnPopUpLoseResource(prod);
                    prod.transform.DOScale(Vector3.one*0.01f, 0.2f).OnComplete(() =>
                    {
                        prod.Release();
                    }).SetEase(Ease.InBack).SetLink(gameObject);
                }).SetLink(prod.gameObject);
            }
        }
    }
}