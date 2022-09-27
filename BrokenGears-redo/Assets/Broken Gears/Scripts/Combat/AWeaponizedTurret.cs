namespace BrokenGears.Combat {
    using Enemies;
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;

    public abstract class AWeaponizedTurret : ATurret {
        [SerializeField] protected AEnemy defaultTarget;
        [SerializeField] protected float range;
        [SerializeField] protected float attackDelay;
        [SerializeField] protected float damage;
        [SerializeField] protected Vector3 rangeOrigin;
        [SerializeField] protected Bone[] bones;

        public abstract UnityEvent OnAttack();
        protected abstract Transform AttackOrigin();

        protected List<AEnemy> enemiesInRange = new List<AEnemy>();
        protected AEnemy target;

        private float attackTimer;

        private void Awake() {
            attackTimer = attackDelay;
        }

        protected virtual void Update() {
            RotateParts();
            CheckTargets();
            AttackLogic();
        }

        protected void RotateParts() {
            if (target) {
                for (int i = 0; i < bones.Length; i++) {
                    bones[i].Rotate(target.Targetpoint);
                }
            }
        }

        protected virtual void CheckTargets() {
            if (!TryGetOverlappingEnemies(out List<AEnemy> enemies)) {
                target = defaultTarget;
                return;
            }

            for (int i = 0; i < enemiesInRange.Count; i++) {
                AEnemy enemy = enemiesInRange[i];

                if (!enemies.Contains(enemy)) {
                    enemiesInRange.Remove(enemy);
                }
            }

            for (int i = 0; i < enemies.Count; i++) {
                AEnemy enemy = enemies[i];

                if (!enemiesInRange.Contains(enemy)) {
                    enemiesInRange.Add(enemy);
                }
            }

            if (enemiesInRange.Count > 0) {
                for (int i = 0; i < enemiesInRange.Count; i++) {
                    if (enemiesInRange[i].IsAlive) {
                        target = enemiesInRange[i];
                        return;
                    }
                }
            }

            target = defaultTarget;
        }

        private bool TryGetOverlappingEnemies(out List<AEnemy> enemies) {
            enemies = new List<AEnemy>();

            if (!EnemyManager.Instance) {
                return false;
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position + rangeOrigin, range, EnemyManager.Instance.Enemylayer);

            for (int i = 0; i < colliders.Length; i++) {
                AEnemy enemy = colliders[i].GetComponentInParent<AEnemy>();
                if (enemy && !enemies.Contains(enemy)) {
                    enemies.Add(enemy);
                }
            }

            return true;
        }

        private void AttackLogic() {
            if (target != defaultTarget && attackTimer >= attackDelay && EnemyManager.Instance) {
                DoAttack();
                attackTimer = 0f;
            }

            if (attackTimer < attackDelay) {
                attackTimer += Time.deltaTime;
            }
        }

        protected virtual void DoAttack() {
            if (AttackOrigin()) {
                if (Physics.Raycast(AttackOrigin().position, AttackOrigin().forward, out RaycastHit hit, EnemyManager.Instance.Enemylayer)) {
                    AEnemy enemy = hit.transform.GetComponentInParent<AEnemy>();
                    if (enemy) {
                        enemy.DoHit(hit.point, damage);
                        if(OnAttack() != null) {
                            OnAttack()?.Invoke();
                        }
                    }
                }
            }
        }

        protected virtual void OnDrawGizmosSelected() {
            if (AttackOrigin()) {
                Debug.DrawRay(AttackOrigin().position, AttackOrigin().forward * range, Color.red);
            }
            Gizmos.DrawWireSphere(transform.position + rangeOrigin, range);
        }

        [System.Serializable]
        protected class Bone {
            [SerializeField] private Transform origin;
            [SerializeField] private Axis axis;
            [SerializeField] private bool useLocal;
            [SerializeField] private float turnSpeed;

            public Transform Origin => origin;
            public Axis Axis => axis;
            public bool UseLocal => useLocal;
            public float TurnSpeed => turnSpeed;

            public void Rotate(Transform target) {
                Vector3 direction = (target.position - origin.position).normalized;
                Quaternion lookRotation = GetLookRotation(direction);
                Vector3 lerpedRotation = Quaternion.Lerp(origin.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;

                Quaternion newRotation = GetNewPartRotation(lerpedRotation);
                ApplyPartRotation(newRotation);
            }

            private static Quaternion GetLookRotation(Vector3 direction) {
                if (direction != Vector3.zero) {
                    return Quaternion.LookRotation(direction);
                }

                return Quaternion.Euler(Vector3.zero);
            }

            private Quaternion GetNewPartRotation(Vector3 lerpedRotation) {
                Quaternion newRotation = Quaternion.identity;

                switch (axis) {
                    case Axis.x:
                        newRotation = Quaternion.Euler(lerpedRotation.x, 0f, 0f);
                        break;
                    case Axis.y:
                        newRotation = Quaternion.Euler(0f, lerpedRotation.y, 0f);
                        break;
                    case Axis.z:
                        newRotation = Quaternion.Euler(0f, 0f, lerpedRotation.z);
                        break;
                }

                return newRotation;
            }

            private void ApplyPartRotation(Quaternion newRotation) {
                if (useLocal) {
                    origin.localRotation = newRotation;
                    return;
                }

                origin.rotation = newRotation;
            }
        }

        protected enum Axis {
            x, y, z
        }
    }
}
