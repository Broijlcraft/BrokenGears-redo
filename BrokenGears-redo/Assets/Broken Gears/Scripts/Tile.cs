namespace BrokenGears {
    using UnityEngine;

    public class Tile : MonoBehaviour {
        [SerializeField] private Tile child;
        [SerializeField] private Tile parent;

        public Tile Child => child;
        public Tile Parent => parent;
        public bool IsOccupied { get; set; }
    }
}