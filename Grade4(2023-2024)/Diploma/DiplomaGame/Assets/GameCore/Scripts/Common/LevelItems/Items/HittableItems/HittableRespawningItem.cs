using System.Collections;
using DG.Tweening;
using GameCore.Common.LevelItems.InteractItems;
using GameBasicsCore.Game.Configs.DataConfigs;
using GameBasicsSignals;
using UnityEngine;

namespace GameCore.Common.LevelItems.Items.HittableItems
{
    public class HittableRespawningItem : SimpleProductingHittableItem
    {
        [SerializeField] private GameObject[] _stages;
        public GameObject[] stages => _stages;
        
        [SerializeField] private UpgradePropertyDataConfig _respawnTimeConfig;
        [SerializeField] private UpgradePropertyDataConfig _respawnStagesCountConfig;
        [SerializeField] private UpgradePropertyDataConfig _respawnDelayConfig;
        [SerializeField] private bool _setNotGrownOnBuy = true;
        [SerializeField] private bool _hideLastStage = true;
        protected float respawnTime => (float) upgradesController.GetModel(_respawnTimeConfig).value;
        protected int respawnStagesCount => upgradesController.GetModel(_respawnStagesCountConfig).valueInt;
        protected float respawnDelay => (float) upgradesController.GetModel(_respawnDelayConfig).value;
        
        private HittableInteractItem _interactItemCached;
        public HittableInteractItem interactItem
        {
            get
            {
                if (_interactItemCached == null) _interactItemCached = GetComponent<HittableInteractItem>();
                return _interactItemCached;
            }
        }

        private int _currentStage;
        public int currentStage => _currentStage;
        
        public readonly TheSignal onStartedRespawn = new();
        public readonly TheSignal onEndedRespawn = new();
        
        private void Awake()
        {
            if (GetComponentInParent<BuyObject>() && _setNotGrownOnBuy)
            {
                GetComponentInParent<BuyObject>().onBuy.Once(() =>
                {
                    SetDisabled();
                    StartRespawn(1);
                });
            }
        }

        public override void OnHit(float multipler = 1)
        {
            base.OnHit(multipler);
            NextStage((int)multipler);
        }

        protected virtual void NextStage(int stagesCnt = 1)
        {
            int targetStage = _currentStage + stagesCnt;
            if (targetStage >= _stages.Length)
            {
                targetStage = _stages.Length - 1;
            }
            for(int i = _currentStage; i < targetStage; i++)
            {
                _stages[i].SetActive(false);
            }
           
            _currentStage = targetStage;
            _stages[_currentStage].SetActive(true);
            
            if (_currentStage >= _stages.Length - 1)
            {
                EffectOnStagesEnded();
                OnStagesEnded();
                onTurnOff.Dispatch();
            }
        }

        protected virtual void OnStagesEnded()
        {
            DOVirtual.DelayedCall(respawnDelay, ()=>
            {
                StartRespawn();
            }, false).SetLink(gameObject);
        }
        
        protected virtual void EffectOnStagesEnded()
        {
            if(enableTween != null) enableTween.Kill();
            SetDisabled();
            interactItem.ExitCurrentInteractor();
            interactItem.enabled = false;

            if (_hideLastStage)
            {
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    visibleObject.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        visibleObject.gameObject.SetActive(false);
                    }).SetLink(gameObject);
                });
            }
        }
        
        protected void StartRespawn(int respawnStageAtStart = 0)
        {
            if (!_hideLastStage)
            {
                visibleObject.DOScale(Vector3.one * 0.01f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    visibleObject.gameObject.SetActive(false);
                    ResetStages(); 
                }).SetLink(gameObject);
            }
            else
            {
                ResetStages(); 
            }
            
            StartCoroutine(RespawnCoroutine(respawnStageAtStart));
            onStartedRespawn.Dispatch();
        }

        private IEnumerator RespawnCoroutine(int respawnStageAtStart = 0)
        {
            if (respawnStageAtStart > 0)
            {
                visibleObject.localScale = Vector3.one * (1f / respawnStagesCount) * respawnStageAtStart;
            }
            else if(_hideLastStage)
            {
                visibleObject.localScale = Vector3.one * 0.01f;
                visibleObject.gameObject.SetActive(false);
            }
            
            visibleObject.localPosition = Vector3.zero;
            float respawnStageTime = respawnTime / respawnStagesCount;
            
            for (int i = respawnStageAtStart; i < respawnStagesCount - 1; i++)
            {
                yield return new WaitForSeconds(respawnStageTime);
                visibleObject.gameObject.SetActive(true);
                
                visibleObject.DOScale(Vector3.one * (1f / respawnStagesCount) * (i + 1), 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);
            }
            yield return new WaitForSeconds(respawnStageTime + 0.5f);
            visibleObject.gameObject.SetActive(true);
            visibleObject.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                interactItem.enabled = true;
                SetEnabled();
                onEndedRespawn.Dispatch();
                onTurnOn.Dispatch();
            }).SetLink(gameObject);
        }

        protected virtual void ResetStages()
        {
            _stages[_currentStage].SetActive(false);
            _currentStage = 0;
            _stages[_currentStage].SetActive(true);
        }
    }
}