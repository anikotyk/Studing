using GameCore.Common.Saves;
using GameBasicsCore.Tools.Extensions;
using UnityEngine;
using Zenject;

namespace GameCore.Common.Installers
{
    public class GCCommonPreProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindDefault<GameSaveData>();
            Container.BindDefault<CharacterTypeSaveData>();
        }
    }
}
