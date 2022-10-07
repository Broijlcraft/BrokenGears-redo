namespace BrokenGears.editor {
    using Data;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(Database))]
    public class DatabaseEditor : Editor {
        private Database database;
        private SerializedProperty createNewOnStart;

        private bool allowDelete;

        public override void OnInspectorGUI() {
            string filePath = database.DataPath;

            EditorGUILayout.BeginHorizontal();
            DrawLabelField("Path", 90f);
            string path = Path.GetDirectoryName(filePath);
            EditorGUILayout.SelectableLabel(path, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            if (GUILayout.Button("Open Location")) {
                System.Diagnostics.Process.Start(path);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(createNewOnStart);

            if (GUILayout.Button("Open XML")) {
                if (File.Exists(filePath)) {
                    System.Diagnostics.Process.Start(filePath);
                } else {
                    Debug.LogWarning("XML Doesn't exist");
                }
            }

            if (GUILayout.Button("Delete XML")) {
                if (File.Exists(filePath)) {
                    allowDelete = !allowDelete;
                } else {
                    Debug.LogWarning("XML Doesn't exist");
                }
            }

            if (allowDelete) {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Confirm")) {
                    if (File.Exists(filePath)) {
                        allowDelete = false;
                        File.Delete(filePath);
                        Debug.LogWarning("Deleted XML");
                    }
                }

                if (GUILayout.Button("Cancel")) {
                    allowDelete = false;
                }

                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawLabelField(string value, float minLabelWidth) {
            EditorGUILayout.LabelField(value, GUILayout.Width(EditorGUIUtility.labelWidth - minLabelWidth));
        }

        private void OnEnable() {
            database = (Database)target;
            allowDelete = false;
            createNewOnStart = serializedObject.FindProperty(nameof(createNewOnStart));
        }

        private void OnDisable() {
            allowDelete = false;
        }
    }
}