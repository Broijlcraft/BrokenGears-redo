namespace BrokenGears {
    using UnityEngine;

    public class PauseManager : MonoBehaviour {
        public static PauseManager Instance { get; private set; }
        public bool IsPaused { get; private set; }

        private void Awake() {
            if (Instance) {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Update() {
            if (Input.GetButtonDown("Cancel")) {
                if (TurretManager.Instance && TurretManager.Instance.SelectedTurret) {
                    return;
                }

                IsPaused = !IsPaused;
            }
        }
    }
}