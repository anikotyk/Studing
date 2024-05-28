using GameCore.ShipScene.Battle.Utilities;
using TMPro;
using UnityEngine;

namespace GameCore.ShipScene.Battle.Waves
{
    public class WaveDisplay : BattleStartEndListener
    {
        [SerializeField] private TMP_Text _text;
        
        protected override void OnBattleStarted(Wave wave)
        {
            _text.text = $"Wave {wave.index + 1}";
        }

        protected override void OnBattleEnded(Wave wave)
        {
        }
    }
}