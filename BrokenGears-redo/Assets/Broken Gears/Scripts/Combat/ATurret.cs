namespace BrokenGears.Combat {
    using Currency;
    using UnityEngine;
    using System.Collections.Generic;

    public abstract class ATurret : MonoBehaviour {
        [SerializeField] private int price;
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField] private Sprite turretImage;

        private bool isPurchased;

        private Tile placedParentTile;
        private List<Material> materials = new List<Material>();

        private const string EmissionKey = "_EmissionColor";

        public int Price => price;
        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public Sprite TurretImage => turretImage;
        public bool IsPurchased => isPurchased;
        public bool IsActive { get; set; }

        protected virtual void Awake() {
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < meshRenderers.Length; i++) {
                Material material = meshRenderers[i].material;
                materials.Add(material);
            }

            EnableEmission();
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
                placedParentTile.OccupyingTurret = null;
            }

            if (tile.Parent) {
                tile = tile.Parent;
            }

            isActive = true;
            isPurchased = true;
            placedParentTile = tile;
            placedParentTile.OccupyingTurret = this;

            if (CurrencyManager.Instance) {
                CurrencyManager.Instance.ChangeScrap(-price);
            }

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
            IsActive = true;
        }
    }
}