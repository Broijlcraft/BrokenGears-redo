namespace BrokenGears.Enemies {
    using UI;
    using UnityEngine;
    using System.Collections.Generic;

    public class EnemyManager : MonoBehaviour {
        [SerializeField] private LayerMask enemylayer;
        [SerializeField] private Transform spawnpoint;

        [SerializeField] private bool canSpawn;
        [SerializeField] private float spawnDelay;
        [SerializeField] private float waveDelay;

        [SerializeField] private EnemyPool[] waves;
        [SerializeField] private Canvas worldCanvas;
        [SerializeField] private EnemyHealthBar enemyHealthBar;

        public static EnemyManager Instance { get; private set; }

        public LayerMask Enemylayer => enemylayer;

        private bool isChangingWave;
        private float spawnTimer;
        private float waveTimer;
        private int waveIndex = 0;

        private List<Pool> waveEnemiesLeft;

        private void Awake() {
            if (Instance) {
                Destroy(this);
                return;
            }

            Instance = this;

            if (waves.Length == 0) {
                enabled = false;
            }
        }

        private void Start() {
            StartWave(waveIndex);
        }

        private void Update() {
            if (!canSpawn) { return; }

            if (!isChangingWave) {
                spawnTimer += Time.deltaTime;

                if (spawnTimer > spawnDelay) {
                    SpawnRandomEnemy();
                    spawnTimer = 0f;
                }

                return;
            }

            waveTimer += Time.deltaTime;

            if (waveTimer > waveDelay) {
                waveTimer = 0f;
                isChangingWave = false;
            }
        }

        private void StartWave(int index) {
            if (waveIndex >= waves.Length) {
                canSpawn = false;
                return;
            }

            waveEnemiesLeft = new List<Pool>();

            Pool[] pools = waves[index].Pools;

            for (int i = 0; i < pools.Length; i++) {
                Pool pool = new Pool();
                pool.prefab = pools[i].prefab;
                pool.amountToSpawn = pools[i].amountToSpawn;

                waveEnemiesLeft.Add(pool);
            }

            waveIndex++;
        }

        private void SpawnRandomEnemy() {
            int randomIndex = Random.Range(0, waveEnemiesLeft.Count);

            AEnemy prefab = waveEnemiesLeft[randomIndex].prefab;
            AEnemy enemy = Instantiate(prefab, spawnpoint.position, spawnpoint.rotation);
            enemy.Init();

            waveEnemiesLeft[randomIndex].amountSpawned++;
            if (waveEnemiesLeft[randomIndex].amountSpawned == waveEnemiesLeft[randomIndex].amountToSpawn) {
                waveEnemiesLeft.RemoveAt(randomIndex);
            }

            if (waveEnemiesLeft.Count == 0) {
                StartWave(waveIndex);
                isChangingWave = true;
            }
        }

        public EnemyHealthBar SpawnEnemyHealthBar() {
            EnemyHealthBar healthBar = Instantiate(enemyHealthBar, worldCanvas.transform);
            return healthBar;
        }
    }
}