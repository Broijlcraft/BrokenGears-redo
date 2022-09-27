namespace BrokenGears.Tools {
    using UnityEngine;
    using UnityEditor;

    public class ScreenShotScript : MonoBehaviour {
        public void MakeScreenShot() {
            print(Application.dataPath);
            string s = System.DateTime.Now.Year.ToString();
            s += System.DateTime.Now.Month.ToString();
            s += System.DateTime.Now.Day.ToString();
            s += System.DateTime.Now.Hour.ToString();
            s += System.DateTime.Now.Minute.ToString();
            s += System.DateTime.Now.Second.ToString();
            s += System.DateTime.Now.Millisecond.ToString();

            ScreenCapture.CaptureScreenshot("Assets/ScreenSHot" + s + ".png");
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ScreenShotScript))]
    public class ScreenShotButton : Editor {
        ScreenShotScript target_ScreenShot;
        public void OnEnable() {
            target_ScreenShot = (ScreenShotScript)target;
        }
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            if (GUILayout.Button("Make screenshot")) {
                target_ScreenShot.MakeScreenShot();
                Debug.LogWarning("Successfully made a screenshot!");
            }
        }

    }
#endif
}