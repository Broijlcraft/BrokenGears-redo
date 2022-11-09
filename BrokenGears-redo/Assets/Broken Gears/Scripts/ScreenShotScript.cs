// Made by Max Broijl
// Editor tool to make screenshots in editor
// Resolution comes from editor resolution

namespace BrokenGears.Utility {
    using System;
    using UnityEngine;
    using UnityEditor;

    using Random = UnityEngine.Random;

    public class ScreenshotScript : MonoBehaviour {
        [SerializeField, Range(1, 4), Tooltip("This increase the resolution of the picture taken \n * value")]
        private int supersize = 1;
        public int Supersize => supersize;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ScreenshotScript))]
    public class ScreenshotButton : Editor {
        private ScreenshotScript screenshotScript;

        private void OnEnable() {
            screenshotScript = (ScreenshotScript)target;
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            if (GUILayout.Button("Make screenshot")) {
                string name = string.Empty;
                Vector2 size = GetMainGameViewSize();
                name += $"@{size.x}x{size.y}";

                if (screenshotScript.Supersize > 1) {
                    name += $"S{screenshotScript.Supersize}";
                }

                name += "#";

                name += DateTime.Now.ToString() + DateTime.Now.Year.ToString();
                name = name.Replace(" ", "").Replace("/", "").Replace(":", "").Replace("-", "");
                name += Random.Range(100000000, 999999999).ToString() + DateTime.Now.Millisecond.ToString() + ".png";
                ScreenCapture.CaptureScreenshot("Assets/" + name, screenshotScript.Supersize);

                Debug.LogWarning("Saving screenshot!");
            }

        }

        private Vector2 GetMainGameViewSize() {
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
            return (Vector2)Res;
        }
    }
#endif
}