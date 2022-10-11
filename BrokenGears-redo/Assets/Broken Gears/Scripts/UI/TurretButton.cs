namespace BrokenGears.UI {
    using Combat;
    using Currency;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class TurretButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private ATurret turret;
        [SerializeField] private Button button;

        private void Start() {
            button.onClick.AddListener(OnClick);
        }

        public void OnPointerEnter(PointerEventData _) {
            SetText(true);
        }

        public void OnPointerExit(PointerEventData _) {
            SetText(false);
        }

        private void OnClick() {
            if (CurrencyManager.Instance && TurretManager.Instance) {
                if (CurrencyManager.Instance.ScrapCount >= turret.BuyPrice) {
                    TurretManager.Instance.SpawnTurret(turret);
                }
            }
        }

        private void SetText(bool enter) {
            if (TurretManager.Instance) {
                TurretManager.Instance.TurretName.text = enter ? turret.DisplayName : string.Empty;
                TurretManager.Instance.TurretPrice.text = enter ? turret.BuyPrice.ToString() : string.Empty;
            }
        }
    }
}