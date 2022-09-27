namespace BrokenGears.Combat {
    using UnityEngine;
    using UnityEngine.Events;

    public class RivetGun : AWeaponizedTurret {
        [SerializeField] private UnityEvent onAttack;
        [SerializeField] private Transform attackOrigin;
        public override UnityEvent OnAttack() => onAttack;
        protected override Transform AttackOrigin() => attackOrigin;
    }
}