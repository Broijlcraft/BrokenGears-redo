namespace BrokenGears {
    using UnityEngine;

    public class PlayerControl : MonoBehaviour {
        [SerializeField] private new Camera camera;

        [SerializeField] private float minCameraZoom;
        [SerializeField] private float defaultCameraZoom;
        [SerializeField] private float cameraZoomSpeed;

        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;

        private float currentCameraZoom;

        private void Start() {
            currentCameraZoom = defaultCameraZoom;
        }

        private void Update() {
            RotateCameraLogic(false);
            ZoomCameraLogic(false);
        }

        private void FixedUpdate() {
            MoveCameraLogic(true);
        }

        public void RotateCameraLogic(bool fixedDeltaTime) {
            float multiplier = rotationSpeed * GetDeltaTime(fixedDeltaTime);

            float x = Input.GetAxisRaw("Mouse X") * multiplier;
            float y = Input.GetAxisRaw("Mouse Y") * multiplier * -1f;

            camera.transform.Rotate(Vector3.right * y);
            transform.Rotate(Vector3.up * x, Space.World);
        }

        private void ZoomCameraLogic(bool fixedDeltaTime) {
            float zoomThisFrame = Input.GetAxisRaw("Mouse ScrollWheel") * cameraZoomSpeed * GetDeltaTime(fixedDeltaTime);
            currentCameraZoom = Mathf.Clamp(currentCameraZoom - zoomThisFrame, minCameraZoom, defaultCameraZoom);
            camera.fieldOfView = currentCameraZoom;
        }

        public void MoveCameraLogic(bool fixedDeltaTime) {
            float multiplier = movementSpeed * GetDeltaTime(fixedDeltaTime);

            float x = Input.GetAxisRaw("Horizontal") * multiplier;
            float y = Input.GetAxisRaw("Vertical") * multiplier;

            Vector3 position = new Vector3(x, 0f, y);
            transform.Translate(position);
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
}