namespace BrokenGears {
    using Combat;
    using UnityEngine;
    using UnityEngine.UI;

    public class TurretManager : MonoBehaviour {
        [SerializeField] private Camera origin;
        [SerializeField] private LayerMask tileLayer;
        [SerializeField] private LayerMask turretLayer;

        [SerializeField] private Color canPlaceColor;
        [SerializeField] private Color canNotPlaceColor;

        [SerializeField] private TurretButton[] turretButtons;
        [SerializeField] private TurretRotation[] turretRotations;

        public static TurretManager Instance { get; private set; }
        public ATurret SelectedTurret { get; private set; }

        private void Awake() {
            if (Instance) {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Start() {
            for (int i = 0; i < turretButtons.Length; i++) {
                turretButtons[i].Init(this);
            }
        }

        private void Update() {
            Ray ray = origin.ScreenPointToRay(Input.mousePosition);

            TryGetMouseHoverTile(ray, out Tile tile, tileLayer);

            if (SelectedTurret) {
                SnapTurretToTile(SelectedTurret, tile);
               
                if (Input.GetButtonDown("Cancel") || Input.GetMouseButtonDown(1)) {
                    SelectTurret(null);
                    return;
                }
            }

            if (Input.GetMouseButtonDown(0)) {
                if (SelectedTurret) {
                    if (!tile.IsOccupied && tile.Parent || tile.Child) {
                        SelectedTurret.PlaceTurret(tile);
                        SelectTurret(null);
                    }
                } else {
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, turretLayer)) {
                        ATurret turret = hit.transform.GetComponentInParent<ATurret>();
                        SelectTurret(turret);
                    }
                }
            }
        }

        private bool TryGetMouseHoverTile(Ray ray, out Tile tile, LayerMask layerMask) {
            tile = null;
            if (Physics.Raycast(ray, out RaycastHit tileHit, Mathf.Infinity, layerMask)) {
                tile = tileHit.transform.GetComponent<Tile>();
                return true;
            }
            return false;
        }

        private void SnapTurretToTile(ATurret turret, Tile tile) {
            if (turret && tile) {
                if (tile.Parent) {
                    tile = tile.Parent;                
                }

                Quaternion rotation = GetTurretRotation(tile);
                turret.transform.SetPositionAndRotation(tile.transform.position, rotation);

                Color color = canNotPlaceColor;
                if (tile.Parent || tile.Child && !tile.IsOccupied) {
                    color = canPlaceColor;
                }

                turret.ChangeColor(color);
            }
        }

        public Quaternion GetTurretRotation(Tile tile) {
            if (tile && tile.Parent || tile.Child) {
                Transform parent = tile.Parent ? tile.Parent.transform : tile.transform;
                Transform child = tile.Child ? tile.Child.transform : tile.transform;

                if (parent.position.x < child.position.x) {
                    return GetTurretRotation(Direction.positive_X);
                }

                if (parent.position.x > child.position.x) {
                    return GetTurretRotation(Direction.negative_X);
                }

                if (parent.position.z < child.position.z) {
                    return GetTurretRotation(Direction.positive_Z);
                }

                if (parent.position.z > child.position.z) {
                    return GetTurretRotation(Direction.negative_Z);
                }
            }

            return SelectedTurret.transform.rotation;
        }

        private Quaternion GetTurretRotation(Direction direction) {
            for (int i = 0; i < turretRotations.Length; i++) {
                if (turretRotations[i].ExtendDirection == direction) {
                    return turretRotations[i].Rotation;
                }
            }
            return Quaternion.identity;
        }

        private void SelectTurret(ATurret turret) {
            if (SelectedTurret) {
                if (SelectedTurret.IsPurchased) {
                    SelectedTurret.ResetToDefault();
                } else {
                    Destroy(SelectedTurret.gameObject);
                }
            }

            SelectedTurret = turret;
            if (SelectedTurret) {
                SelectedTurret.SetActive(false);
            }
        }

        private void TrySpawnTurret(ATurret turret) {
            ATurret spawnedTurret = Instantiate(turret);
            SelectTurret(spawnedTurret);
        }

        [System.Serializable]
        private class TurretButton {
            [SerializeField] private Button button;
            [SerializeField] private ATurret prefab;

            public void Init(TurretManager manager) {
                button.onClick.AddListener(() => manager.TrySpawnTurret(prefab));
            }
        }

        [System.Serializable]
        private class TurretRotation {
            [SerializeField] private Vector3 rotation;
            [SerializeField] private Direction extendDirection;

            public Quaternion Rotation => Quaternion.Euler(rotation);
            public Direction ExtendDirection => extendDirection;
        }

        private enum Direction {
            positive_X, negative_X, positive_Z, negative_Z
        }
    }
}