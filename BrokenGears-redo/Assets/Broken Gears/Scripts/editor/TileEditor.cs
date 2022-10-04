namespace BrokenGears.editor {
    using UnityEditor;
    using System.Collections.Generic;

    [CustomEditor(typeof(Tile))]
    public class TileEditor : Editor {
        Tile tile;

        public override void OnInspectorGUI() {
            List<string> exludes = new List<string>();

            if(tile.Parent && !tile.Child) {
                exludes.Add("child");
            }

            if(tile.Child && !tile.Parent) {
                exludes.Add("parent");
            }
            
            DrawPropertiesExcluding(serializedObject, exludes.ToArray());
            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable() {
            tile = (Tile)target;
        }
    }
}