namespace BrokenGears.Pathing {
    using UnityEngine;

    public class WaypointsManager : MonoBehaviour {

        [SerializeField] private Transform[] waypoints;

        public static WaypointsManager Instance { get; private set; }
        public Transform[] Waypoints => waypoints;

        private void Awake() {
            if (Instance) {
                Destroy(this);
                return;
            }

            Instance = this;
        }
    }
}