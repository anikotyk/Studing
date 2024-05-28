using GameCore.ShipScene.Battle.Utilities;
using GameCore.ShipScene.Battle.Waves;
using UnityEngine;

namespace GameCore.ShipScene.Sounds
{
    public class ShipBackgroundMusicManager : BattleStartEndListener
    {
        [SerializeField] private AudioSource _music;

        protected override void OnBattleStarted(Wave wave)
        {
            _music.Play();
        }

        protected override void OnBattleEnded(Wave wave)
        {
            _music.Stop();
        }
    }
}