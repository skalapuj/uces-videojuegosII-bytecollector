using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ByteCollector.UI
{
    public class SettingsManager : MonoBehaviour
    {
        [Header("Referencias de UI")]
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullscreenToggle;

        private Resolution[] availableResolutions;
        private List<Resolution> filteredResolutions;

        private void Start()
        {
            SetupResolutions();
            SetupFullscreenToggle();
        }

        private void SetupResolutions()
        {
            if (resolutionDropdown == null) return;

            availableResolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;

            for (int i = 0; i < availableResolutions.Length; i++)
            {
                Resolution res = availableResolutions[i];

                // Filtramos para evitar entradas duplicadas con distinta tasa de refresco (Hz)
                bool alreadyExists = filteredResolutions.Exists(r => r.width == res.width && r.height == res.height);
                if (!alreadyExists)
                {
                    filteredResolutions.Add(res);
                    string option = $"{res.width} x {res.height}";
                    options.Add(option);

                    if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
                    {
                        currentResolutionIndex = filteredResolutions.Count - 1;
                    }
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();

            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }

        private void SetupFullscreenToggle()
        {
            if (fullscreenToggle == null) return;

            fullscreenToggle.isOn = Screen.fullScreen;
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }

        public void SetResolution(int resolutionIndex)
        {
            if (resolutionIndex < 0 || resolutionIndex >= filteredResolutions.Count) return;

            Resolution selectedResolution = filteredResolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);

            // Verificación en consola
            Debug.Log($"[Settings] Resolución cambiada a: {selectedResolution.width} x {selectedResolution.height}");
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;

            // Verificación en consola
            Debug.Log($"[Settings] Pantalla completa: {isFullscreen}");
        }
    }
}