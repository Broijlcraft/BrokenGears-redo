namespace BrokenGears {
    using Combat;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    public class TurretManager : MonoBehaviour {
        [SerializeField] private Camera origin;
        [SerializeField] private LayerMask tileLayer;
        [SerializeField] private LayerMask turretLayer;

        [SerializeField] private Color canPlaceColor;
        [SerializeField] private Color canNotPlaceColor;

        [SerializeField] private Text turretName;
        [SerializeField] private Text turretPrice;

        [SerializeField] private TurretInfo turretInfo;

        [SerializeField] private TurretRotation[] turretRotations;

        public static TurretManager Instance { get; private set; }
        public ATurret SelectedTurret { get; private set; }
        public bool IsShowingInfo { get; private set; }
        public Text TurretName => turretName;
        public Text TurretPrice => turretPrice;

        private void Awake() {
            if (Instance) {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Start() {
            TurretName.text = string.Empty;
            TurretPrice.text = string.Empty;

            turretInfo.Init();
        }

        private void Update() {
            if (IsShowingInfo) { return; }

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
                if (SelectedTurret && tile) {
                    if (tile.Parent || tile.Child) {
                        if (!tile.OccupyingTurret || tile.OccupyingTurret == SelectedTurret) {
                            SelectedTurret.PlaceTurret(tile);
                            SelectTurret(null);
                        }
                    }
                } else {
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, turretLayer)) {
                        ATurret turret = hit.transform.GetComponentInParent<ATurret>();
                        SelectTurret(turret);
                        turretInfo.Enable(true);
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
            if (turret && !turret.IsActive && tile) {
                if (tile.Parent) {
                    tile = tile.Parent;
                }

                Quaternion rotation = GetTurretRotation(tile);
                turret.transform.SetPositionAndRotation(tile.transform.position, rotation);

                Color color = canNotPlaceColor;
                if ((tile.Parent || tile.Child) && (!tile.OccupyingTurret || tile.OccupyingTurret == SelectedTurret)) {
                    color = canPlaceColor;
                }

                turret.ChangeColor(color);
            }
        }

        public Quaternion GetTurretRotation(Tile tile) {
            if (tile && (tile.Parent || tile.Child)) {
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
        }

        public void SpawnTurret(ATurret turret) {
            ATurret spawnedTurret = Instantiate(turret);
            SelectTurret(spawnedTurret);
        }

        [System.Serializable]
        private class TurretRotation {
            [SerializeField] private Vector3 rotation;
            [SerializeField] private Direction extendDirection;

            public Quaternion Rotation => Quaternion.Euler(rotation);
            public Direction ExtendDirection => extendDirection;
        }

        [System.Serializable]
        public class TurretInfo {
            [SerializeField] private GameObject holder;

            [SerializeField] private Text turretName;
            [SerializeField] private Image turretIcon;
            [SerializeField] private Image turretImage;

            [SerializeField] private Button moveButton;
            [SerializeField] private Button closeButton;

            public void Init() {
                TrySetButton(moveButton, MoveTurret);
                TrySetButton(closeButton, () => Enable(false));
            }

            public void Enable(bool on) {
                if (Instance) {
                    Instance.IsShowingInfo = on;

                    if (on) {
                        TrySetText(turretName, Instance.SelectedTurret.DisplayName);
                        TrySetImage(turretIcon, Instance.SelectedTurret.Icon);
                        TrySetImage(turretImage, Instance.SelectedTurret.TurretImage);
                    } else if (Instance.SelectedTurret.IsActive) {
                        Instance.SelectTurret(null);
                    }
                }

                holder.SetActive(on);
            }

            private void TrySetText(Text text, string content) {
                if (text) {
                    text.text = string.IsNullOrEmpty(content) ? "Turret" : content;
                }
            }

            private void TrySetImage(Image image, Sprite sprite) {
                if (image) {
                    image.sprite = sprite;
                }
            }

            private void TrySetButton(Button button, UnityAction action) {
                if (button && action != null) {
                    button.onClick.AddListener(action);
                }
            }

            private void MoveTurret() {
                Instance.SelectedTurret.IsActive = false;
                Instance.IsShowingInfo = false;
                Enable(false);
            }

        }

        private enum Direction {
            positive_X, negative_X, positive_Z, negative_Z
        }
    }
}