using UnityEngine;

namespace WinterUniverse
{
    public class WeaponSlot : MonoBehaviour
    {
        [HideInInspector] public Character Owner;
        [HideInInspector] public WeaponItemData Data;
        [HideInInspector] public MeleeWeaponDamageCollider MeleeWeaponDamageCollider;
        public HandSlotType Type;

        private GameObject _model;

        private void OnEnable()
        {
            Owner = GetComponentInParent<Character>();
        }

        public void Equip(WeaponItemData weapon)
        {
            if (weapon == null)
            {
                return;
            }
            Data = weapon;// TODO add instantiate?
            foreach (StatModifierCreator creator in Data.Modifiers)
            {
                Owner.StatModule.AddStatModifier(creator);
            }
            _model = Instantiate(Data.Model, transform);// TODO pool spawn
            _model.transform.SetLocalPositionAndRotation(Data.LocalPosition, Data.LocalRotation);
            MeleeWeaponDamageCollider = _model.GetComponentInChildren<MeleeWeaponDamageCollider>();// TODO change force setup to setup via data/slot reference
            MeleeWeaponDamageCollider.Setup(this);
        }

        public void Unequip()
        {
            if (Data == null)
            {
                return;
            }
            foreach (StatModifierCreator creator in Data.Modifiers)
            {
                Owner.StatModule.RemoveStatModifier(creator);
            }
            Data = null;
            if (_model != null)
            {
                Destroy(_model);// TODO pool despawn
            }
        }
    }
}