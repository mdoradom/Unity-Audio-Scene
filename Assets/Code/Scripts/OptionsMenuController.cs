using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    
    [Header("Mouse Settings")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;
    
    [Header("UI Elements")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button applyButton;
    
    [Header("References")]
    [SerializeField] private MainMenuController mainMenuController;
    
    private const string MasterVolume = "MasterVolume";
    private const string MusicVolume = "MusicVolume";
    private const string SFXVolume = "SFXVolume";
    private const string MouseSensitivity = "MouseSensitivity";
    
    private void Start()
    {
        // Set up button listeners
        backButton.onClick.AddListener(ReturnToMainMenu);
        applyButton.onClick.AddListener(SaveSettings);
        
        // Set up slider listeners
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        
        // Load saved settings
        LoadSettings();
    }
    
    private void LoadSettings()
    {
        // Set sliders to saved values or defaults
        masterVolumeSlider.value = PlayerPrefs.GetFloat(MasterVolume, 0.75f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolume, 0.75f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(SFXVolume, 0.75f);
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat(MouseSensitivity, 1.0f);
        
        // Apply loaded values
        SetMasterVolume(masterVolumeSlider.value);
        SetMusicVolume(musicVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);
        SetMouseSensitivity(mouseSensitivitySlider.value);
    }
    
    private void SaveSettings()
    {
        // Save settings to PlayerPrefs
        PlayerPrefs.SetFloat(MasterVolume, masterVolumeSlider.value);
        PlayerPrefs.SetFloat(MusicVolume, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(SFXVolume, sfxVolumeSlider.value);
        PlayerPrefs.SetFloat(MouseSensitivity, mouseSensitivitySlider.value);
        PlayerPrefs.Save();
    }
    
    public void SetMasterVolume(float volume)
    {
        // Convert slider value (0 to 1) to decibels (-80 to 0)
        masterMixer.SetFloat(MasterVolume, Mathf.Log10(volume) * 20);
    }
    
    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat(MusicVolume, Mathf.Log10(volume) * 20);
    }
    
    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat(SFXVolume, Mathf.Log10(volume) * 20);
    }
    
    public void SetMouseSensitivity(float sensitivity)
    {
        if (sensitivityValueText != null)
            sensitivityValueText.text = sensitivity.ToString("F1");
    }
    
    public void ReturnToMainMenu()
    {
        SaveSettings();
        mainMenuController.ShowMainMenu();
    }
}