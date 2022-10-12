namespace BrokenGears.UI {
    using UnityEngine;
    using UnityEngine.UI;

    public class DroppedScrap : MonoBehaviour {
        [SerializeField] private float destroyTime;

        [SerializeField] private Text scrapText;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject holder;

        public void Init(Vector3 position, int amount) {
            transform.position = position + offset;
            scrapText.text = amount.ToString();
            holder.SetActive(true);
            animator.SetTrigger("Fade");
            Destroy(gameObject, destroyTime);
        }

        private void LateUpdate() {
            transform.LookAt(Camera.main.transform);
        }
    }
}