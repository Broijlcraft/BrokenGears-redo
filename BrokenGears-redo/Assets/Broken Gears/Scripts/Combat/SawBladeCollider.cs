namespace BrokenGears.Combat {
    using Enemies;
    using UnityEngine;

    public class SawBladeCollider : MonoBehaviour {
        public AEnemy Enemy { get; private set; }

        private bool overlappingThisFrame;

        private void LateUpdate() {
            print(Enemy);
            if (overlappingThisFrame) {
                overlappingThisFrame = false;
                return;
            }

            Enemy = null;
        }

        private void OnTriggerStay(Collider other) {
            if (!other.CompareTag("Enemy")) { return; }
            
            AEnemy enemy = other.GetComponentInParent<AEnemy>();

            if (enemy) {
                Enemy = enemy;
                overlappingThisFrame = true;
            }
        }
    }
}