using NaughtyAttributes;
using GameBasicsCore.Game.Misc;
using UnityEngine;

namespace GameCore.ShipScene.Extentions
{
    public class ReversibleAnimatorApplier : AnimatorParameterApplier
    {
        [Space]
        [SerializeField, ShowIf(nameof(showInt))] protected int _reversedIntValue;
        [SerializeField, ShowIf(nameof(showFloat))] protected int _reversedFloatValue;

        public int defaultInt => _valueInt;
        public float defaultFloat => _valueFloat;
        public bool defaultBool => _valueBool;
        
        public bool showInt => IsInt();
        public bool showFloat => IsFloat();
        
        public void Revert()
        {
            switch (_valueType)
            {
                case ValueType.Int:
                    ApplyAsInt(_reversedIntValue);
                    break;
                case ValueType.Float:
                    ApplyAsFloat(_reversedFloatValue);
                    break;
                case ValueType.Bool:
                    ApplyAsBool(!_valueBool);
                    break;
            }
        }
    }
}