using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    [SerializeField] GameObject resolutionDropdown;
    [SerializeField] GameObject fullscreenToggle;
    [SerializeField] GameObject soundSlider;
    [SerializeField] GameObject musicSlider;
    private TMP_Dropdown resolutionDropdownComponent;
    private Toggle fullscreenToggleComponenet;
    private Slider soundSliderComponent;
    private Slider musicSliderComponent;
    private List<Resolution> resolutionList;
    void OnEnable()
    {
        gameManager = GameManager.instance;
        audioManager = AudioManager.instance;
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
        resolutionList = new List<Resolution>();
        Resolution currentResolution = Screen.currentResolution;
        int i = 0;
        int currentIndex = 0;
        foreach (var res in Screen.resolutions)
        {
            if (ResolutionDifferentThenLastInList(res))
            {
                resolutionList.Add(res);
                options.Add(res.width + " x " + res.height);
                if (currentResolution.width == res.width && currentResolution.height == res.height)
                {
                    currentIndex = i;
                }
                i++;
            }
            
        }
        resolutionDropdownComponent.ClearOptions();
        resolutionDropdownComponent.AddOptions(options);
        resolutionDropdownComponent.value = currentIndex;
        resolutionDropdownComponent.RefreshShownValue();
    }

    private bool ResolutionDifferentThenLastInList(Resolution res)
    {
        if(resolutionList.Count == 0)
        {
            return true;
        }
        else
        {
            Resolution lastResolution = resolutionList[resolutionList.Count - 1];
            return res.width != lastResolution.width || res.height != lastResolution.height;
        }
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
        UpdateSoundVolume();
        UpdateMusicVolume();
        WritePlayerPrefs();
    }

    public void UpdateSoundVolume()
    {
        float soundVolume = soundSliderComponent.value;
        SetSoundVolume(soundVolume);
    }

    public void UpdateMusicVolume()
    {
        float musicVolume = musicSliderComponent.value;
        SetMusicVolume(musicVolume);
    }

    private void UpdateScreenResolution()
    {
        int resIndex = resolutionDropdownComponent.value;
        //Resolution resolution = resolutions[resIndex];
        Resolution resolution = resolutionList[resIndex];
        bool fullscreen = fullscreenToggleComponenet.isOn;
        Screen.SetResolution(resolution.width, resolution.height, fullscreen);
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
        return audioManager.MusicVolume;
    }

    private void SetMusicVolume(float newVolume)
    {
        audioManager.MusicVolume = newVolume;
    }

    private float GetSoundVolume()
    {
        return audioManager.SoundVolume;
    }

    private void SetSoundVolume(float newVolume)
    {
        audioManager.SoundVolume = newVolume;
    }
}
