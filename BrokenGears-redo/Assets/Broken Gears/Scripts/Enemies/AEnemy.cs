namespace BrokenGears.Enemies {
    using UnityEngine;
    using UnityEngine.Events;

    public abstract class AEnemy : MonoBehaviour {
        [SerializeField] private Transform targetpoint;

        protected float currentHealth;

        public Transform Targetpoint => targetpoint;
        public bool IsAlive { get; private set; }
        
        protected abstract float DefaultHealth();
        public abstract HealthEvent Events();

        protected virtual void Awake() {
            if (!targetpoint) {
                targetpoint = transform;
            }
        }

        private void Start() {
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

            if(currentHealth == 0) {
                DoDeath();
            }
        }

        private void OnDeath_Internal() {
            IsAlive = false;
        }

        [System.Serializable]
        public class HealthEvent {
            public UnityEvent OnDeath;
            public UnityEvent<float> OnDamage;
            public UnityEvent<Vector3, float> OnHit;
        }
    }
}