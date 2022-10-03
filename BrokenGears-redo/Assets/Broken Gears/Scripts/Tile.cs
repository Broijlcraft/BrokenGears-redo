namespace BrokenGears {
    using UnityEngine;
    using System.Collections.Generic;

    public class Tile : MonoBehaviour {

        public int xIndex;
        public int zIndex;
        public List<NeighbourTile> neighbours1 = new List<NeighbourTile>();

        public void Init(Vector3 position, int xIndex, int zIndex, int listIndex) {
            transform.position = position;
            gameObject.isStatic = true;
            this.xIndex = xIndex;
            this.zIndex = zIndex;
            name += listIndex;
        }

        [System.Serializable]
        public class NeighbourTile {
            [SerializeField, HideInInspector] private string name;
            [SerializeField] private Tile tile;
            [SerializeField] private Direction direction;

            public Tile Tile => tile;
            public Direction Direction => direction;

            public NeighbourTile(Tile tile, Direction direction) {
                name = direction.ToString();
                this.tile = tile;
                this.direction = direction;
            }
        }
    }
}