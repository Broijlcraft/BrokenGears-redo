namespace BrokenGears {
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class InputManager : MonoBehaviour {
        public static InputManager Instance { get; private set; }

        private Inputs inputs;
        private InputAction movement;
        private InputAction rotationDeltaX;
        private InputAction rotationDeltaY;

        public Vector3 RotationDeltaAxis;

        private void Awake() {
            if (Instance != null) {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            inputs = new Inputs();
        }

        private void OnEnable() {
            movement = inputs.PlayerControls.Movement;
            movement.Enable();

            rotationDeltaX = inputs.PlayerControls.RotationDeltaX;
            rotationDeltaX.Enable();

            rotationDeltaY = inputs.PlayerControls.RotationDeltaY;
            rotationDeltaY.Enable();

            rotationDeltaX.performed += ctx => RotationDeltaAxis.y = ctx.ReadValue<float>();
            rotationDeltaY.performed += ctx => RotationDeltaAxis.x = ctx.ReadValue<float>();
        }

        private void OnDisable() {
            movement.Disable();
            rotationDeltaX.Disable();
            rotationDeltaY.Disable();
        }
    }
}