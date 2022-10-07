namespace BrokenGears.UI {
    using Data;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Audio;
    using UnityEngine.Events;
    using System.Collections.Generic;

    using DropdownOption = UnityEngine.UI.Dropdown.OptionData;

    public class SettingsMenu : MonoBehaviour {
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Dropdown resolutionsDropdown;

        [SerializeField] private Slider mouseSensitivity;

        [SerializeField] private Slider masterVolume;
        [SerializeField] private Slider musicVolume;
        [SerializeField] private Slider sfxVolume;

        [SerializeField] private AudioMixer audioMixer;

        private int resolutionsIndex;
        private List<Resolution> resolutions = new List<Resolution>();

        private void Start() {
            if (!Database.Instance) {
                return;
            }

            SetSliders();
            SetToggles();
            SetResolutionsDropdown();
        }

        private void SetSliders() {
            Settings settings = Database.Instance.Settings;

            SetSlider(mouseSensitivity, settings.MouseSensitivity, FloatSettingsType.MouseSensitivity, ChangeMouseSensitivity);
            SetSlider(masterVolume, settings.MasterVolume, FloatSettingsType.MasterVolume, ChangeMasterVolume);
            SetSlider(musicVolume, settings.MusicVolume, FloatSettingsType.MusicVolume, ChangeMusicVolume);
            SetSlider(sfxVolume, settings.SfxVolume, FloatSettingsType.SfxVolume, ChangeSfxVolume);
        }

        private void SetToggles() {
            SetToggle(fullscreenToggle, ChangeFullscreen, Screen.fullScreen);
        }

        private void SetSlider(Slider slider, float startValue, FloatSettingsType type, UnityAction<float> action) {
            switch (type) {
                case FloatSettingsType.MouseSensitivity:
                    if (PlayerControl.Instance) {
                        PlayerControl.Instance.MouseSensitivity = startValue;
                    }
                    break;
                case FloatSettingsType.MasterVolume: case FloatSettingsType.MusicVolume: case FloatSettingsType.SfxVolume:
                    ChangeAudioMixerValue(startValue, type);
                    break;
            }

            if (slider && action != null) {
                slider.value = startValue;
                slider.onValueChanged.AddListener(action);
            }
        }

        private void SetToggle(Toggle toggle, UnityAction<bool> action, bool startValue) {
            if(toggle && action != null) {
                toggle.isOn = startValue;
                toggle.onValueChanged.AddListener(action);
            }
        }

        private void SetResolutionsDropdown() {
            if (resolutionsDropdown) {
                resolutions = Screen.resolutions.ToList();
                resolutions.Reverse();
                if (resolutionsDropdown) {
                    List<DropdownOption> options = new List<DropdownOption>();
                    List<string> stringOptions = new List<string>();

                    for (int i = 0; i < resolutions.Count; i++) {
                        string resolutionString = resolutions[i].width + " x " + resolutions[i].height;
                        if (!stringOptions.Contains(resolutionString)) {
                            DropdownOption option = new DropdownOption() {
                                text = resolutionString
                            };
                            options.Add(option);
                            stringOptions.Add(resolutionString);

                            if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height) {
                                resolutionsIndex = i;
                            }
                        }
                    }

                    resolutionsDropdown.options = options;
                    resolutionsDropdown.SetValueWithoutNotify(resolutionsIndex);
                    resolutionsDropdown.RefreshShownValue();
                    resolutionsDropdown.onValueChanged.AddListener(ChangeResolution);
                }
            }
        }

        private void ChangeFloatSettingsValue(float value, FloatSettingsType type) {
            switch (type) {
                case FloatSettingsType.MouseSensitivity:
                    Database.Instance.Settings.MouseSensitivity = value;
                    if (PlayerControl.Instance) {
                        PlayerControl.Instance.MouseSensitivity = value;
                    }
                    break;
                case FloatSettingsType.MasterVolume:
                    Database.Instance.Settings.MasterVolume = value;
                    ChangeAudioMixerValue(value, FloatSettingsType.MasterVolume);
                    break;
                case FloatSettingsType.MusicVolume:
                    Database.Instance.Settings.MusicVolume = value;
                    ChangeAudioMixerValue(value, FloatSettingsType.MusicVolume);
                    break;
                case FloatSettingsType.SfxVolume:
                    Database.Instance.Settings.SfxVolume = value;
                    ChangeAudioMixerValue(value, FloatSettingsType.SfxVolume);
                    break;
            }

            Database.Instance.Save();
        }

        private void ChangeAudioMixerValue(float value, FloatSettingsType type) {
            string key = type.ToString();
            audioMixer.SetFloat(key, Mathf.Log10(value) * 20f);
        }

        private void ChangeToggleSettingsValue(bool value, BoolSettingsType type) {
            switch (type) {
                case BoolSettingsType.Fullscreen:
                    Screen.fullScreen = value;
                    break;
            }
        }

        private void ChangeMouseSensitivity(float value) {
            ChangeFloatSettingsValue(value, FloatSettingsType.MouseSensitivity);
        }

        private void ChangeMasterVolume(float value) {
            ChangeFloatSettingsValue(value, FloatSettingsType.MasterVolume);
        }

        private void ChangeMusicVolume(float value) {
            ChangeFloatSettingsValue(value, FloatSettingsType.MusicVolume);
        }

        private void ChangeSfxVolume(float value) {
            ChangeFloatSettingsValue(value, FloatSettingsType.SfxVolume);
        }

        private void ChangeFullscreen(bool value) {
            ChangeToggleSettingsValue(value, BoolSettingsType.Fullscreen);
        }

        private void ChangeResolution(int index) {
            Resolution resolution = resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        private enum FloatSettingsType {
            MouseSensitivity,
            MasterVolume,
            MusicVolume,
            SfxVolume
        }

        private enum BoolSettingsType {
            Fullscreen
        }
    }
}