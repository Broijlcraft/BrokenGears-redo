namespace BrokenGears {
    using UnityEngine;
    using System.Collections;

    public class InputsTest : MonoBehaviour {

        [SerializeField] private Transform player;
        [SerializeField] private new Camera camera;
        [SerializeField] private float speed;

        private float x;
        private float y;

        private void Start() {
            StartCoroutine(LockCursorCor());
        }

        private IEnumerator LockCursorCor() {
            yield return new WaitForSeconds(.1f);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update() {
            x = Input.GetAxisRaw("Mouse Y");
            y = Input.GetAxisRaw("Mouse X");
        }

        private void FixedUpdate() {
            //float x = InputManager.single.RotationDeltaAxis.x;
            //float y = InputManager.single.RotationDeltaAxis.y;

            //camera.transform.Rotate(Vector3.left * x * speed * Time.fixedDeltaTime);
            //camera.transform.Rotate(Vector3.up * y * speed * Time.fixedDeltaTime, Space.World);

            camera.transform.Rotate(Vector3.left * x * speed * Time.fixedDeltaTime);
            player.transform.Rotate(Vector3.up * y * speed * Time.fixedDeltaTime);
        }
    }
}