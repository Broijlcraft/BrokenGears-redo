namespace BrokenGears {
    using UnityEngine;

    public class PlayerControl : MonoBehaviour {
        [SerializeField] private new Camera camera;

        [SerializeField] private float minCameraZoom;
        [SerializeField] private float defaultCameraZoom;
        [SerializeField] private float cameraZoomSpeed;

        [SerializeField] private float movementSpeed;
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private float verticalCameraTopClamp;
        [SerializeField] private float verticalCameraBottomClamp;

        private float currentCameraZoom;
        private float verticalCameraValue;

        public float MouseSensitivity { get => mouseSensitivity;  set => mouseSensitivity = value; }

        public static PlayerControl Instance { get; private set; }

        private void Awake() {
            if (Instance) {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Start() {
            currentCameraZoom = defaultCameraZoom;
            verticalCameraValue = camera.transform.eulerAngles.x * -1;
        }

        private void Update() {
            if (Input.GetButton("Left Control")) {
                RotateCameraLogic(false);
            }
            ZoomCameraLogic(false);
        }

        private void FixedUpdate() {
            MoveCameraLogic(true);
        }

        public void RotateCameraLogic(bool fixedDeltaTime) {
            HorizontalCameraRotation(fixedDeltaTime);
            VerticalCameraRotation(fixedDeltaTime);
        }

        private void HorizontalCameraRotation(bool fixedDeltaTime) {
            float x = Input.GetAxisRaw("Mouse X") * mouseSensitivity * 10f * GetDeltaTime(fixedDeltaTime);
            transform.Rotate(Vector3.up * x, Space.World);
        }

        private void VerticalCameraRotation(bool fixedDeltaTime) {
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * mouseSensitivity * GetDeltaTime(fixedDeltaTime);
            verticalCameraValue += mouseY;

            if (verticalCameraValue > verticalCameraTopClamp) {
                verticalCameraValue = verticalCameraTopClamp;
                mouseY = 0f;
                ClampXRotationAxisToValue(camera.transform, -verticalCameraTopClamp);
            } else if (verticalCameraValue < -verticalCameraBottomClamp) {
                verticalCameraValue = -verticalCameraBottomClamp;
                mouseY = 0f;
                ClampXRotationAxisToValue(camera.transform, verticalCameraBottomClamp);
            }

            camera.transform.Rotate(Vector3.left * mouseY);
        }

        private void ClampXRotationAxisToValue(Transform transform_, float value) {
            Vector3 eulerRotation = transform_.localEulerAngles;
            eulerRotation.x = value;
            transform_.localEulerAngles = eulerRotation;
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