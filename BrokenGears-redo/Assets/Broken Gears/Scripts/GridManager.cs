namespace BrokenGears {
    using System.Collections.Generic;
    using UnityEngine;

    public class GridManager : MonoBehaviour {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Tile tilePrefab;

        //public List<Tile> tiles = new List<Tile>();
        public Vector2Int GridSize => gridSize;
        public Tile TilePrefab => tilePrefab;


        //private void Start() {
        //    for (int i = 0; i < tiles.Count; i++) {
        //        TrySetDirectionalTile(i, i - 1, Direction.centerLeft, tiles, out Tile tile){

        //        }
        //    }
        //}

        //private Tile TrySetDirectionalTile(int tileIndex, int directionIndex, Direction direction, List<Tile> tiles, out Tile tile) {
        //    tile = null;

        //    return false;
        //}

        private void OnDrawGizmosSelected() {
            Vector3 size = new Vector3(gridSize.x, 0, gridSize.y);
            Gizmos.DrawWireCube(transform.position, size);
        }
    }

    public enum Direction {
        topLeft,
        topCenter,
        topRight,
        centerLeft,
        centerRight,
        bottomLeft,
        bottomCenter,
        bottomRight
    }
}