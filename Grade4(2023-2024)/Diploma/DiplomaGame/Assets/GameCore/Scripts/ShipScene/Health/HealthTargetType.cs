using System.Collections.Generic;

namespace GameCore.ShipScene
{
    public enum HealthTargetType
    {
        Any,
        Enemy,
        Alias,
        Weapon,
        Ship
    }
    
    public static class HealthTargetTypeExtensions
    {
        public static bool CanApplyDamage(this HealthTargetType targetType, Health health)
        {
            return targetType == HealthTargetType.Any || targetType == health.targetType;
        }

        public static bool CanApplyDamage(this List<HealthTargetType> targetTypes, Health health)
        {
            return targetTypes.Has(x => x.CanApplyDamage(health));
        }
        
        public static bool CanApplyDamage(this HealthTargetType[] targetTypes, Health health)
        {
            return targetTypes.Has(x => x.CanApplyDamage(health));
        }
    }
}