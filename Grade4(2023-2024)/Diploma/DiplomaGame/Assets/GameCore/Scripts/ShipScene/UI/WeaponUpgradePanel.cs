using GameCore.ShipScene.DataConfigs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.ShipScene.UI
{
    public class WeaponUpgradePanel : UpgradePanelShipCoins
    {
        [SerializeField] private Image _weaponImage;
        [SerializeField] private TextMeshProUGUI _weaponName;
        [SerializeField] private WeaponsGroupDataConfig _weaponsGroupDataConfig;

        protected override void Validate(bool animate)
        {
            base.Validate(animate);
            int index = _weaponsGroupDataConfig.Count <= model.level ? model.level - 1 :  model.level;
            _weaponImage.sprite = _weaponsGroupDataConfig[index].icon;
            _weaponName.text = _weaponsGroupDataConfig[index].name;
        }
    }
}