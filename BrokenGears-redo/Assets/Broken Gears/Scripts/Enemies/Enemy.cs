namespace BrokenGears.Enemies {
    using UI;
    using UnityEngine;

    public class Enemy : AEnemy {
        [SerializeField] private float defaultHealth;
        [SerializeField] private Animator animator;
        [SerializeField] private HealthEvent events;

        protected override float DefaultHealth() => defaultHealth;
        public override HealthEvent Events() => events;
        private EnemyHealthBar healthBar;

        protected override void Awake() {
            base.Awake();
            events.OnDeath.AddListener(OnDeath);
        }

        public override void Init() {
            if (!EnemyManager.Instance) { return; }

            healthBar = EnemyManager.Instance.SpawnEnemyHealthBar();
            healthBar.Init(this);
        }

        private void OnDeath() {
            Destroy(gameObject, 1f);
            animator.SetTrigger("Death");
        }
    }
}