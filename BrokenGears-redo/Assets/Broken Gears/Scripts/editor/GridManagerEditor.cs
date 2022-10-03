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
                Vector2Int gridSize = gridManager.GridSize;
                List<Tile> tiles = CreateGrid(gridSize, gridManager.TilePrefab, gridManager.transform);

                for (int i = 0; i < tiles.Count; i++) {
                    int topLeftIndex = i - gridSize.y - 1;
                    TrySetDirectionalTile(i, topLeftIndex, tiles, Direction.topLeft, gridSize);

                    int topCenterIndex = i - gridSize.y;
                    TrySetDirectionalTile(i, topCenterIndex, tiles, Direction.topCenter, gridSize);

                    int topRightIndex = i - gridSize.y + 1;
                    TrySetDirectionalTile(i, topRightIndex, tiles, Direction.topRight, gridSize);

                    int centerLeftIndex = i - 1;
                    TrySetDirectionalTile(i, centerLeftIndex, tiles, Direction.centerLeft, gridSize);

                    int centerRightIndex = i + 1;
                    TrySetDirectionalTile(i, centerRightIndex, tiles, Direction.centerRight, gridSize);
                }

                //TrySetDirectionalTile(1, 0, tiles, Direction.centerLeft, out Tile a);
                //tiles[1].centerLeft = a;

                //Debug.Log(a);
                //for (int i = 0; i < tiles.Count; i++) {
                //    Tile tile = tiles[i];

                //    int centerLeftIndex = i - 1;
                //    if(TrySetDirectionalTile(i, centerLeftIndex, tiles, Direction.centerLeft, out Tile tile_CL)) {
                //        EditorUtility.SetDirty(tile);
                //        tile.centerLeft = tile_CL;
                //    }

                //    int centerRightIndex = i + 1;
                //    if(TrySetDirectionalTile(i, centerRightIndex, tiles, Direction.centerRight, out Tile tile_CR)) {
                //        EditorUtility.SetDirty(tile);
                //        tile.centerRight = tile_CR;
                //    }
                //}
            }

            if (GUILayout.Button("Clear Grid")) {
                ClearGrid();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void TrySetDirectionalTile(int tileIndex, int directionalIndex, List<Tile> tiles, Direction direction, Vector2Int gridSize) {
                Tile tile = tiles[tileIndex];
            if (directionalIndex >= 0 && directionalIndex < tiles.Count && directionalIndex < tile.zIndex + gridSize.x * tile.xIndex) {
                Tile directionalTile = tiles[directionalIndex];

                tile.neighbours1.Add(new Tile.NeighbourTile(directionalTile, direction));
            }
        }

        private List<Tile> CreateGrid(Vector2Int gridSize, Tile tilePrefab, Transform parent) {
            int i = 0;
            List<Tile> tiles = new List<Tile>();
            for (int xIndex = 0; xIndex < gridSize.x; xIndex++) {
                for (int zIndex = 0; zIndex < gridSize.y; zIndex++) {
                    Tile tile = Instantiate(tilePrefab, parent);

                    float xFloat = (parent.position.x - (gridSize.x / 2)) + xIndex + .5f/*half tilesize*/;
                    float zFloat = (parent.position.z - (gridSize.y / 2)) + zIndex + .5f/*half tilesize*/;

                    tile.Init(new Vector3(xFloat, 0, zFloat), xIndex, zIndex, i);
                    //tile.transform.position = new Vector3(xFloat, 0, zFloat);
                    //tile.gameObject.isStatic = true;
                    //tile.name += i;
                    //tile.xIndex = xIndex;
                    //tile.zIndex = zIndex;
                    i++;
                    tiles.Add(tile);
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