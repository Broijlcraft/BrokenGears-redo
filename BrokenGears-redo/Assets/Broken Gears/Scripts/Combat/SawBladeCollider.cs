namespace BrokenGears.Combat {
    using System.Collections;
    using System.Collections.Generic;
    using Enemies;
    using UnityEngine;

    public class SawBladeCollider : MonoBehaviour {
        public AEnemy Enemy { get; private set; }

        private void OnTriggerStay(Collider other) {
            AEnemy enemy = other.GetComponentInParent<AEnemy>();

            if (enemy) {
                Enemy = enemy;
                return;
            }

            Enemy = null;
        }
    }
}