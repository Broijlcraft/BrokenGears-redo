namespace BrokenGears {
    using Combat;
    using UnityEngine;

    public class Tile : MonoBehaviour {
        [SerializeField] private Tile child;
        [SerializeField] private Tile parent;

        public Tile Child => child;
        public Tile Parent => parent;
        public ATurret OccupyingTurret { get; set; }
    }
}