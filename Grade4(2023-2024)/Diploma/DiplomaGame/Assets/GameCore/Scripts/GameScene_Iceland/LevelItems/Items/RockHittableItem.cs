using GameCore.Common.LevelItems.Items.HittableItems;
using GameCore.Common.Misc;
using UnityEngine;

namespace GameCore.GameScene_Iceland.LevelItems.Items
{
    public class RockHittableItem : HittableRespawningItem
    {
        [SerializeField] private float _spawnZDelta = 0.5f;
        [SerializeField] private float _spawnZMaxCoef = -1f;
        [SerializeField] private float _spawnXDelta = 0f;
        [SerializeField] private float _spawnXMaxCoef = 1f;
        public override CharacterTools.HittingToolType toolType => CharacterTools.HittingToolType.Pick;
        public override Vector3 helperPosition => view.position + Vector3.back*0.85f;
        public override float canHitAngle => 60f;
        protected override void SpawnProduction(int cnt)
        {
            spawnProductsManager.SpawnBunchAtPoint(spawnProductConfig, pointForSpawn.position + Vector3.up * 0.1f,
                cnt, -spawnPosRange, _spawnXMaxCoef* spawnPosRange+ _spawnXMaxCoef *_spawnXDelta, -spawnPosRange, _spawnZMaxCoef*spawnPosRange + _spawnZMaxCoef *_spawnZDelta);
        }
    }
}