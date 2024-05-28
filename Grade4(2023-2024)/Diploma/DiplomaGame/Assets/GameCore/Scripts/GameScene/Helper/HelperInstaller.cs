using GameBasicsCore.Tools.Extensions;
using GameBasicsSDK.Modules.IdleArcade.Installers.CharacterContext;
using GameBasicsSDK.Modules.IdleArcade.Models;
using UnityEngine;

namespace GameCore.GameScene.Helper
{
    public class HelperInstaller : InteractorCharacterInstaller<HelperView>
    {
        [SerializeField] private bool _enableInteractionValidator;

        public override void InstallBindings()
        {
            base.InstallBindings();

            if (_enableInteractionValidator)
            {
                Container.BindDefault<HelperInteractionValidateController>();
            }
        }

        protected override void BindCharacterModel()
        {
            Container.BindInterfacesAndBaseAndSelfTo<InteractorCharacterModel, HelperInteractorModel>().AsSingle().NonLazy();
        }
    }
}