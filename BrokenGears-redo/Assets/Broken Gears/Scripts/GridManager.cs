namespace BrokenGears {
    using UnityEngine;

    public class GridManager : MonoBehaviour {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Tile tilePrefab;

        public Vector2Int GridSize => gridSize;
        public Tile TilePrefab => tilePrefab;

        private void OnDrawGizmosSelected() {
            Vector3 size = new Vector3(gridSize.x, 0, gridSize.y);
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}