using GameCore.GameScene.LevelItems;
using GameBasicsCore.Game.Misc;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Base;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.Characters.Modules.Movings;
using UnityEngine;

namespace GameCore.Common.LevelItems.Character.Modules
{
    public class FishingCharacterModule : InteractorCharacterModule
    {
        [SerializeField] private GameObject _fishingGO;
        [SerializeField] private FishingRod _fishingRod;
        [SerializeField] private AnimatorParameterApplier _fishingAnim;
        [SerializeField] private AnimatorParameterApplier _endFishingAnim;
        private bool _isFishing;

        public override void Construct()
        {
            base.Construct();
            character.GetModule<CharacterMovingModule>().onStartMoving.On(EndFishing);
        }

        public void StartFishing(Transform characterPoint, FishingRod fishingRod)
        {
            if(_isFishing) return;
            _isFishing = true;
            character.transform.position = characterPoint.position;
            character.transform.rotation = characterPoint.rotation;
            _fishingRod.Initialize(fishingRod);
            _fishingGO.SetActive(true);
            _fishingAnim.Apply();
            
            _fishingRod.carrier.onChange.On(CheckCanContinueFishing);
        }

        private void CheckCanContinueFishing()
        {
            if (!_fishingRod.carrier.HasSpace())
            {
                EndFishing();
            }
        }
        
        public void EndFishing()
        {
            if(!_isFishing) return;
            _isFishing = false;
            _fishingGO.SetActive(false);
            _endFishingAnim.Apply();
            _fishingRod.carrier.onChange.Off(CheckCanContinueFishing);
        }
    }
}