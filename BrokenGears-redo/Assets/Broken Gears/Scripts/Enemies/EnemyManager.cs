namespace BrokenGears.Enemies {
    using UnityEngine;

    public class EnemyManager : MonoBehaviour {

        [SerializeField] private LayerMask enemylayer;
        [SerializeField] private Transform spawnpoint;

        public static EnemyManager Instance { get; private set; }
        public LayerMask Enemylayer => enemylayer;

        private void Awake() {
            if (Instance) {
                Destroy(this);
                return;
            }

            Instance = this;
        }
    }
}