namespace BrokenGears.Data {
    using UnityEngine;
    using System.IO;
    using System.Xml.Serialization;

    public class Database : MonoBehaviour {
        [SerializeField] private bool createNewOnStart;

        private bool shouldSave;
        private const string settingsDatabaseName = "Settings";
        private readonly XmlSerializer serializer = new XmlSerializer(typeof(Settings));

        public static Database Instance { get; private set; }
        public Settings Settings { get; private set; }

        public string DataPath => Path.Combine(Application.persistentDataPath, settingsDatabaseName);

        private void Awake() {
            if (Instance) {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);

            if (createNewOnStart) {
                CreateNewDatabase();
            } else {
                LoadData();
            }
        }

        private void LateUpdate() {
            if (shouldSave) {
                shouldSave = false;
                SaveData();
            }
        }

        /// <summary>
        /// Use this to trigger data save request
        /// </summary>
        public void Save() {
            shouldSave = true;
        }

        public void LoadData() {
            try {
                using (FileStream stream = new FileStream(DataPath, FileMode.Open)) {
                    Settings = serializer.Deserialize(stream) as Settings;
                    stream.Close();
                }
            } catch {
                CreateNewDatabase();
            }
        }

        private void CreateNewDatabase() {
            Debug.LogWarning("Creating new Database!");
            Settings = new Settings() {
                MouseSensitivity = 15,
                MasterVolume = .2f,
                MusicVolume = .2f,
                SfxVolume = .2f
            };
            Save();
        }

        /// <summary>
        /// Use this only once, use Save() after
        /// </summary>
        private void SaveData() {
            using (FileStream stream = new FileStream(DataPath, FileMode.Create)) {
                serializer.Serialize(stream, Settings);
                stream.Close();
            }
        }

    }

    public class Settings {
        public float MouseSensitivity { get; set; }

        public float MasterVolume { get; set; }
        public float MusicVolume { get; set; }
        public float SfxVolume { get; set; }
    }
}