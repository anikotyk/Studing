using DG.Tweening;
using GameCore.Common.LevelItems.Items.HittableItems;

namespace GameCore.GameScene_Island.LevelItems.Items
{
    public class PalmHittableItem : HittableRespawningItem
    {
        protected override void NextStage(int stagesCnt = 1)
        {
            base.NextStage(stagesCnt);
            
            float deltaY = stages[currentStage].transform.position.y - transform.position.y;
            visibleObject.DOMoveY(-deltaY, 0.25f).SetRelative(true).SetEase(Ease.InOutBack).SetLink(gameObject);
        }
        
        protected override void ResetStages()
        {
            base.ResetStages();
            foreach (var obj in stages)
            {
                obj.SetActive(true);
            }
        }
    }
}