namespace BrokenGears.Enemies {
    using Currency;
    using UnityEngine;
    using UnityEngine.Events;

    public abstract class AEnemy : MonoBehaviour {
        [SerializeField] private Transform targetpoint;
        [SerializeField] protected int scrapDroppedOnDeath;

        protected float currentHealth;

        public float NormalizedHealthAmount { get; private set; }
        public bool IsAlive { get; private set; }
        public Transform Targetpoint => targetpoint;

        protected abstract float DefaultHealth();
        public abstract HealthEvent Events();

        public virtual void Init() { }

        protected virtual void Awake() {
            if (!targetpoint) {
                targetpoint = transform;
            }

            if (Events() != null) {
                currentHealth = DefaultHealth();
                IsAlive = true;

                Events().OnHit.AddListener(OnHit_Internal);
                Events().OnDamage.AddListener(OnDamage_Internal);
                Events().OnDeath.AddListener(OnDeath_Internal);
            }
        }

        public void DoHit(Vector3 position, float amount) {
            Events()?.OnHit?.Invoke(position, amount);
        }

        public void DoDamage(float amount) {
            if (IsAlive) {
                Events()?.OnDamage?.Invoke(amount);
            }
        }        

        public void DoDeath() {
            Events()?.OnDeath?.Invoke();
        }

        private void OnHit_Internal(Vector3 position, float amount) {
            DoDamage(amount);
        }

        private void OnDamage_Internal(float amount) {
            currentHealth = Mathf.Clamp(currentHealth - amount, 0, DefaultHealth());

            NormalizedHealthAmount = Mathf.Clamp01(currentHealth / DefaultHealth());

            if(currentHealth == 0) {
                DoDeath();
            }
        }

        private void OnDeath_Internal() {
            IsAlive = false;

            if (CurrencyManager.Instance) {
                CurrencyManager.Instance.ChangeScrap(scrapDroppedOnDeath);
            }
        }

        [System.Serializable]
        public class HealthEvent {
            public UnityEvent OnDeath;
            public UnityEvent<float> OnDamage;
            public UnityEvent<Vector3, float> OnHit;
        }
    }
}