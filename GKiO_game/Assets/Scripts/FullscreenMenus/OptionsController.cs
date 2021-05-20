using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] GameObject resolutionDropdown;
    [SerializeField] GameObject fullscreenToggle;
    [SerializeField] GameObject soundSlider;
    [SerializeField] GameObject musicSlider;
    private TMP_Dropdown resolutionDropdownComponent;
    private Toggle fullscreenToggleComponenet;
    private Slider soundSliderComponent;
    private Slider musicSliderComponent;
    Resolution[] resolutions;
    void OnEnable()
    {
        gameManager = GameManager.instance;
        GetTheComponents();
        SetupResolutions();
        SetupFullScreenToggle();
        SetupSoundSlider();
        SetupMusicSlider();
    }

    private void GetTheComponents()
    {
        resolutionDropdownComponent = resolutionDropdown.GetComponent<TMP_Dropdown>();
        fullscreenToggleComponenet = fullscreenToggle.GetComponent<Toggle>();
        soundSliderComponent = soundSlider.GetComponent<Slider>();
        musicSliderComponent = musicSlider.GetComponent<Slider>();
    }

    private void SetupResolutions()
    {
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        Resolution currentResolution = Screen.currentResolution;
        int i = 0;
        int currentIndex = 0;
        foreach (var res in resolutions)
        {
            options.Add(res.width + " x " + res.height);
            if(currentResolution.width == res.width && currentResolution.height == res.height)
            {
                currentIndex = i;
            }
            i++;
        }
        resolutionDropdownComponent.ClearOptions();
        resolutionDropdownComponent.AddOptions(options);
        resolutionDropdownComponent.value = currentIndex;
        resolutionDropdownComponent.RefreshShownValue();
    }

    private void SetupFullScreenToggle()
    {
        fullscreenToggleComponenet.isOn = Screen.fullScreen;
    }

    private void SetupSoundSlider()
    {
        soundSliderComponent.value = GetSoundVolume();
    }

    private void SetupMusicSlider()
    {
        musicSliderComponent.value = GetMusicVolume();
    }

    public void Back()
    {
        gameObject.SetActive(false);
    }

    public void Apply()
    {
        UpdateScreenResolution();
        UpdateAudio();
        WritePlayerPrefs();
    }

    private void UpdateScreenResolution()
    {
        int resIndex = resolutionDropdownComponent.value;
        Resolution resolution = resolutions[resIndex];
        bool fullscreen = fullscreenToggleComponenet.isOn;
        Screen.SetResolution(resolution.width, resolution.height, fullscreen);
    }

    private void UpdateAudio()
    {
        float musicVolume = musicSliderComponent.value;
        float soundVolume = soundSliderComponent.value;
        SetMusicVolume(musicVolume);
        SetSoundVolume(soundVolume);
    }

    private void WritePlayerPrefs()
    {
        Resolution currentResolution = Screen.currentResolution;
        PlayerPrefs.SetInt("width", currentResolution.width);
        PlayerPrefs.SetInt("height", currentResolution.height);
        int fullscreenPref = Screen.fullScreen ? 1 : 0;
        PlayerPrefs.SetInt("fullscreen", fullscreenPref);
        PlayerPrefs.SetFloat("musicVolume", GetMusicVolume());
        PlayerPrefs.SetFloat("soundVolume", GetSoundVolume());
        PlayerPrefs.Save();
    }

    private float GetMusicVolume()
    {
        return 0.7f;
    }

    private void SetMusicVolume(float newVolume)
    {
        
    }

    private float GetSoundVolume()
    {
        return 0.7f;
    }

    private void SetSoundVolume(float newVolume)
    {

    }
}
