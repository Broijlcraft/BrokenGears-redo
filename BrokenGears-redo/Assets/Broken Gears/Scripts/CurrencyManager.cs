namespace BrokenGears.Currency {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class CurrencyManager : MonoBehaviour {
        [SerializeField] private int startScrap = 25;
        [SerializeField] private Text scrapText;
        public static CurrencyManager Instance { get; private set; }
        
        private const int maxScrap = 999;
        
        private int scrapCount;

        public int ScrapCount => scrapCount;

        private void Awake() {
            if (Instance) {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Start() {
            ChangeScrap(startScrap); ;
        }

        public void ChangeScrap(int amount) {
            scrapCount += amount;
            ClampScrap();
            UpdateUI();
        }

        private void ClampScrap() {
            scrapCount = Mathf.Clamp(scrapCount, 0, maxScrap);
        }

        private void UpdateUI() {
            scrapText.text = "Scrap: " + scrapCount;
        }
    }
}