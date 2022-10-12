namespace BrokenGears.UI {
    using Enemies;
    using UnityEngine;
    using UnityEngine.UI;

    public class EnemyHealthBar : MonoBehaviour {
        [SerializeField] private Image fill;
        [SerializeField] private Vector3 offset;

        private AEnemy enemy;

        public void Init(Enemy enemy) {
            this.enemy = enemy;
            enemy.Events().OnDamage.AddListener(UpdateFill);
            enemy.Events().OnDeath.AddListener(() => Destroy(this.gameObject));
        }

        private void LateUpdate() {
            if (enemy) {
                transform.position = enemy.transform.position + offset;
                transform.LookAt(enemy.transform);
            }
        }

        private void UpdateFill(float dmg) {
            fill.fillAmount = enemy.NormalizedHealthAmount;
        }
    }
}