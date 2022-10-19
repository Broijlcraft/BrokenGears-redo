namespace BrokenGears.UI {
    using UnityEngine;
    using UnityEngine.UI;

    public class EscapedMenu : MonoBehaviour {
        [SerializeField] private Image[] escapees;

        [SerializeField] private Color defaultColor;
        [SerializeField] private Color escapedColor;

        public static EscapedMenu Instance { get; private set; }
        public int Escaped { get; private set; }

        private void Awake() {
            if (Instance) {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Start() {
            for (int i = 0; i < escapees.Length; i++) {
                SetColor(escapees[i], defaultColor);
            }
        }

        public void AddEscapee() {
            if(Escaped < escapees.Length) {
                SetColor(escapees[Escaped], escapedColor);
                Escaped++;
            }
        }

        private void SetColor(Image image, Color color) {
            if (image) {
                image.color = color;
            }
        }
    }
}