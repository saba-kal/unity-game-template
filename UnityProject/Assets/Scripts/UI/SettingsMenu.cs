using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private TMP_Dropdown _graphicsDropdown;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _fullscreenToggle;

    private void Start()
    {
        SetupVolumeSlider(_masterVolumeSlider, "MasterVolume");
        SetupVolumeSlider(_musicVolumeSlider, "MusicVolume");
        SetupVolumeSlider(_sfxVolumeSlider, "SfxVolume");
        SetupGraphicsDropdown();
        SetupResolutionDropdown();
        SetupFullsceeenToggle();
    }

    private void SetupVolumeSlider(
        Slider slider,
        string rtpcName)
    {
        slider.value = PlayerPrefs.GetFloat(rtpcName, 75);
        AkSoundEngine.SetRTPCValue(rtpcName, slider.value);
        slider.onValueChanged.AddListener((value) =>
        {
            AkSoundEngine.SetRTPCValue(rtpcName, slider.value);
            PlayerPrefs.SetFloat(rtpcName, slider.value);
        });
    }

    private void SetupGraphicsDropdown()
    {
        var qualityLevel = QualitySettings.GetQualityLevel();
        _graphicsDropdown.value = qualityLevel;
        _graphicsDropdown.onValueChanged.AddListener((index) =>
        {
            QualitySettings.SetQualityLevel(index);
        });
        _graphicsDropdown.RefreshShownValue();
    }

    private void SetupFullsceeenToggle()
    {
        _fullscreenToggle.onValueChanged.AddListener((value) =>
        {
            Screen.fullScreen = value;
        });
        _fullscreenToggle.isOn = Screen.fullScreen;
    }

    private void SetupResolutionDropdown()
    {
        _resolutionDropdown.ClearOptions();

        var resolutionOptions = new List<string>();

        var currentResolutionIndex = 0;
        var resIndex = 0;
        var addedResolutions = new List<(int, int)>();

        foreach (var resolution in Screen.resolutions)
        {
            if (addedResolutions.Contains((resolution.width, resolution.height)))
            {
                continue;
            }
            addedResolutions.Add((resolution.width, resolution.height));

            resolutionOptions.Add($"{resolution.width} x {resolution.height}");

            if (resolution.width == Screen.width &&
                resolution.height == Screen.height)
            {
                currentResolutionIndex = resIndex;
            }

            resIndex++;
        }

        _resolutionDropdown.AddOptions(resolutionOptions);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();

        _resolutionDropdown.onValueChanged.AddListener((resolutionIndex) =>
        {
            var newResolution = addedResolutions[resolutionIndex];
            Screen.SetResolution(newResolution.Item1, newResolution.Item2, Screen.fullScreen);
        });
    }
}
