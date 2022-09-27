namespace BrokenGears {
    using UnityEngine;

    public class PlayerControl : MonoBehaviour {

        [SerializeField] private ControlType controlType;
        [Space]
        [SerializeField, Tooltip("old middle mouseclick control")] private CameraControl mm_cameraControl;

        private void Update() {
            switch (controlType) {
                case ControlType.mm_cameraControl:
                    mm_cameraControl.RotateCamera(true);
                    break;
            }
        }

        private void FixedUpdate() {
            switch (controlType) {
                case ControlType.mm_cameraControl:
                    mm_cameraControl.Move(true);
                    break;
            }
        }

        private enum ControlType {
            mm_cameraControl,
        }

        [System.Serializable]
        public class CameraControl {
            [SerializeField] private Camera camera;
            [SerializeField] private Transform camHold;

            [SerializeField] private ControlSettings movement;
            [SerializeField] private ControlSettings rotation;

            public Camera Camera => camera;
            public Transform CamHold => camHold;

            public void Move(bool fixedDeltaTime) {
                float multiplier = movement.Speed * GetDeltaTime(fixedDeltaTime);

                float x = GetAxis("Horizontal") * multiplier * (movement.InvertHorizontal ? -1f : 1f);
                float y = GetAxis("Vertical") * multiplier * (movement.InvertVertical ? -1f : 1f);

                Vector3 position = new Vector3(x, 0f, y);
                camHold.Translate(position);
            }

            public void RotateCamera(bool fixedDeltaTime) {
                float multiplier = rotation.Speed * GetDeltaTime(fixedDeltaTime);

                float x = GetAxis("Mouse X") * multiplier * (rotation.InvertHorizontal ? -1f : 1f);
                float y = GetAxis("Mouse Y") * multiplier * (rotation.InvertVertical ? -1f : 1f);

                camera.transform.Rotate(Vector3.right * y);
                camHold.Rotate(Vector3.up * x, Space.World);
            }

            private float GetAxis(string name) {
                float value;
                if (rotation.Acceleration) {
                    value = Input.GetAxis(name);
                } else {
                    value = Input.GetAxisRaw(name);
                }
                return value;
            }

            private float GetDeltaTime(bool fixedDeltaTime) {
                float deltaTime;
                if (fixedDeltaTime) {
                    deltaTime = Time.fixedDeltaTime;
                } else {
                    deltaTime = Time.deltaTime;
                }
                return deltaTime;
            }
        }

        [System.Serializable]
        private class ControlSettings {
            [SerializeField] private float speed;
            [SerializeField] private bool invertHorizontal;
            [SerializeField] private bool invertVertical;
            [SerializeField] private bool acceleration;

            public float Speed => speed;
            public bool InvertHorizontal => invertHorizontal;
            public bool InvertVertical => invertVertical;
            public bool Acceleration => acceleration;
        }
    }
}