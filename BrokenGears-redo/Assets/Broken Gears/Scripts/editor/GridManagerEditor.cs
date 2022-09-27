namespace BrokenGears.editor {
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : Editor {
        private GridManager gridManager;

        public override void OnInspectorGUI() {
            serializedObject.Update();
            DrawDefaultInspector();

            if (GUILayout.Button("Create Grid")) {
                ClearGrid();
                CreateGrid(gridManager.GridSize, gridManager.TilePrefab, gridManager.transform);
            }

            if (GUILayout.Button("Clear Grid")) {
                ClearGrid();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private List<Tile> CreateGrid(Vector2Int gridSize, Tile tilePrefab, Transform parent) {
            List<Tile> tiles = new List<Tile>();
            for (int xIndex = 0; xIndex < gridSize.x; xIndex++) {
                for (int zIndex = 0; zIndex < gridSize.y; zIndex++) {
                    Tile tile = Instantiate(tilePrefab, parent);

                    float xFloat = (parent.position.x - (gridSize.x / 2)) + xIndex + .5f/*half tilesize*/;
                    float zFloat = (parent.position.z - (gridSize.y / 2)) + zIndex + .5f/*half tilesize*/;

                    tile.transform.position = new Vector3(xFloat, 0, zFloat);
                    tiles.Add(tile);
                    tile.gameObject.isStatic = true;
                    EditorUtility.SetDirty(tile);
                }
            }
            return tiles;
        }

        private List<Tile> ClearGrid() {
            List<Tile> tiles = new List<Tile>();
            int tileCount = gridManager.transform.childCount;

            for (int i = 0; i < tileCount; i++) {
                Tile tile = gridManager.transform.GetChild(i).GetComponent<Tile>();
                if (tile) {
                    tiles.Add(tile);
                }
            }

            for (int i = 0; i < tiles.Count; i++) {
                if (tiles[i]) {
                    EditorUtility.SetDirty(tiles[i]);
                    DestroyImmediate(tiles[i].gameObject);
                }
            }
            return new List<Tile>();
        }
        private void OnEnable() {
            gridManager = (GridManager)target;
        }
    }
}