namespace BrokenGears.UI {
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using UnityEngine.Events;

    public class MainMenu : MonoBehaviour {
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        private void Start() {
            SetButton(startButton, StartGame);
            SetButton(quitButton, Quit);
        }

        private void SetButton(Button button, UnityAction action) {
            if(button && action != null) {
                button.onClick.AddListener(action);
            }
        }

        private void StartGame() {
            SceneManager.LoadScene("DEV_Gameplay");
        }

        private void Quit() {
            Application.Quit();
        }
    }
}