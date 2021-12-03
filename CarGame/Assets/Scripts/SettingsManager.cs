﻿// manages all the settings and option panel
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class SettingsManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown textureQualityDropdown;
    public Dropdown antialiasingDropdown;
    public Dropdown vSyncDropdown;
    public Slider audioVolumeSlider;
    public Button applyButton;
    public Canvas canvasOptions;
    public Button exitGame;

    public AudioSource audioSource;
    public Resolution[] resolutions;
    public GameSettings gameSettings;

    private void OnEnable()
    {
        gameSettings = new GameSettings();
        // Creates event listeners
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        audioVolumeSlider.onValueChanged.AddListener(delegate { OnAudioVolumeChange(); });
        applyButton.onClick.AddListener(delegate { OnApplyButton(); });
        exitGame.onClick.AddListener(delegate { OnExitGame(); });

        // gets the resolutions for your computer
        resolutions = Screen.resolutions;
        foreach(Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
        // loads last applyed settings
        LoadSettings();
    }
    public void OnFullscreenToggle() // Fullscreen event handeler
    {

       gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }
    public void OnResolutionChange() // resolution event handeler
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }
    public void OnTextureQualityChange() // texture event handeler
    {
       QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
        
    }
    public void OnAntialiasingChange() // antialiasing event handeler
    {
       gameSettings.antialiasing =  QualitySettings.antiAliasing = (int)Mathf.Pow(2, antialiasingDropdown.value);
    }
    public void OnVSyncChange() // VSyenc event handeler
    {
        QualitySettings.vSyncCount = gameSettings.Vsync = vSyncDropdown.value;
    }
    public void OnAudioVolumeChange() // audio slider event handeler
    {
       audioSource.volume = gameSettings.musicVolume = audioVolumeSlider.value;
    }
    public void OnApplyButton() // apply button event handeler
    {
        //saves the settings to a file
        SaveSettings();
    }
    public void OnExitGame()
    {
        Application.Quit();
    }
    // saves the game settings as a JSON to the Application persistent Data Path
    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath +"/gamesettings.json", jsonData);
    }
    // reads a JSON from Application persistent Data Path and enters it into gameSettings
    public void LoadSettings()
    {
        if(File.Exists(Application.persistentDataPath + "/gamesettings.json"))
        {
            gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
            audioVolumeSlider.value = gameSettings.musicVolume;
            antialiasingDropdown.value = gameSettings.antialiasing;
            vSyncDropdown.value = gameSettings.Vsync;
            resolutionDropdown.value = gameSettings.resolutionIndex;
            fullscreenToggle.isOn = gameSettings.fullscreen;

            resolutionDropdown.RefreshShownValue();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape") && canvasOptions.enabled == false)
        {
            canvasOptions.enabled = true;
        }else if (Input.GetKeyDown("escape") && canvasOptions.enabled == true)
        {
            canvasOptions.enabled = false;
        }
    }
}
