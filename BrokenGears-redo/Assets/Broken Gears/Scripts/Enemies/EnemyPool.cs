namespace BrokenGears.Enemies {
    using UnityEngine;

    [CreateAssetMenu(menuName = "Broken Gears/new Enemypool", fileName = "new Enemypool")]
    public class EnemyPool : ScriptableObject {
        [SerializeField] private Pool[] pools;

        public Pool[] Pools => pools;
    }

    [System.Serializable]
    public class Pool {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int amount;

        public GameObject Prefab => prefab;
        public int Amount => amount;
    }
}