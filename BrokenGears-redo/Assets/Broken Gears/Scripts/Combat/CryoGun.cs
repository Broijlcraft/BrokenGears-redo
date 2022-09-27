namespace BrokenGears.Combat {
    using UnityEngine;
    using UnityEngine.Events;

    public class CryoGun : AWeaponizedTurret {
        [SerializeField] private Transform attackOrigin;
        protected override Transform AttackOrigin() => attackOrigin;
        public override UnityEvent OnAttack() => null;
    }
}