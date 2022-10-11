namespace BrokenGears.UI {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public class PauseMenu : MonoBehaviour {
        public UnityEvent<bool> OnPauseChanged = new UnityEvent<bool>();

        public static PauseMenu Instance { get; private set; }
        public bool IsPaused { get; private set; }

        private void Awake() {
            if (Instance) {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Start() {
            OnPauseChanged.AddListener(OnPause_Internal);
        }

        private void Update() {
            if (Input.GetButtonDown("Cancel") && CanPause()) {
                Pause(!IsPaused);
            }
        }

        private bool CanPause() {
            bool canPause = true;

            if (TurretManager.Instance) {
                canPause = !TurretManager.Instance.SelectedTurret && !TurretManager.Instance.IsShowingInfo;
            }

            return canPause;
        }

        public void Pause(bool pause) {
            IsPaused = pause;
            OnPauseChanged?.Invoke(pause);
        }

        private void OnPause_Internal(bool value) {
            Time.timeScale = value ? 0f : 1f;
        }
    }
}