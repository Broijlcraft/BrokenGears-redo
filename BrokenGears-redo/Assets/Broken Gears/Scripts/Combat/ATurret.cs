namespace BrokenGears.Combat {
    using UnityEngine;
    using System.Collections.Generic;

    public abstract class ATurret : MonoBehaviour {
        [SerializeField] private bool isActive;
        [SerializeField] private bool isPurchased;

        private Tile placedParentTile;
        private List<Material> materials = new List<Material>();

        private const string EmissionKey = "_EmissionColor";

        public bool IsPurchased => isPurchased;
        protected bool IsActive => IsPurchased && isActive;

        protected virtual void Awake() {
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < meshRenderers.Length; i++) {
                Material material = meshRenderers[i].material;
                materials.Add(material);
            }

            EnableEmission();
        }

        private void Start() {
            if (IsActive) {
                PlaceTurret(placedParentTile);
            }
        }

        public bool SetActive(bool value) {
            return isActive = value;
        }

        public void ChangeColor(Color color) {
            for (int i = 0; i < materials.Count; i++) {
                materials[i].SetColor(EmissionKey, color);
            }
        }

        private void EnableEmission() {
            for (int i = 0; i < materials.Count; i++) {
                Material material = materials[i];
                material.EnableKeyword(EmissionKey);
            }
        }

        public void PlaceTurret(Tile tile) {
            if (placedParentTile) {
                placedParentTile.IsOccupied = false;
            }

            if (tile.Parent) {
                tile = tile.Parent;
            }

            isActive = true;
            isPurchased = true;
            placedParentTile = tile;
            placedParentTile.IsOccupied = true;

            ResetToDefault();
        }

        public void ResetToDefault() {
            Vector3 position = placedParentTile.transform.position;
            Quaternion rotation = Quaternion.identity;
            if (TurretManager.Instance) {
                rotation = TurretManager.Instance.GetTurretRotation(placedParentTile);
            }

            transform.SetPositionAndRotation(position, rotation);

            ChangeColor(Vector4.zero);
            SetActive(true);
        }
    }
}