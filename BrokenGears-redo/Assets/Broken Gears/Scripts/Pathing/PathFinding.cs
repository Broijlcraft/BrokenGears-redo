namespace BrokenGears.Pathing {
    using Enemies;
    using UnityEngine;

    public class PathFinding : MonoBehaviour {
        [SerializeField] private float movementSpeed = 1f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private Transform model;
        [SerializeField] private AEnemy enemy;

        private Transform[] waypoints;
        private Transform currentWaypoint;
        private int waypointIndex;
        
        void Start() {
            TryGetWaypoints();
        }

        void Update() {
            if (currentWaypoint && enemy.IsAlive) {
                Vector3 direction = (currentWaypoint.position - transform.position).normalized;
                transform.Translate(direction * Time.deltaTime * movementSpeed);

                Quaternion lookRotation = Quaternion.LookRotation(direction);
                Vector3 rotationToLook = Quaternion.Lerp(model.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
                model.rotation = Quaternion.Euler(0f, rotationToLook.y, 0f);

                if (Vector3.Distance(transform.position, currentWaypoint.position) < .1f) {
                    SetNextWaypoint();
                }
            }
        }

        private void TryGetWaypoints() {
            if (WaypointsManager.Instance) {
                waypoints = WaypointsManager.Instance.Waypoints;
                SetNextWaypoint();
            }
        }

        private void SetNextWaypoint() {
            if (waypoints.Length > 0 && waypointIndex < waypoints.Length) {
                currentWaypoint = waypoints[waypointIndex];
                waypointIndex++;
                return;
            }
            currentWaypoint = null;
        }
    }
}