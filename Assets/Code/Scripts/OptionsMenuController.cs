using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionsMenuController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer globalMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider ambienceVolumeSlider;
    
    // Add text fields for displaying volume values
    [SerializeField] private TextMeshProUGUI masterVolumeText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private TextMeshProUGUI ambienceVolumeText;
    
    [Header("Mouse Settings")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;
    
    [Header("UI Elements")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button applyButton;
    
    [Header("References")]
    [SerializeField] private MainMenuController mainMenuController;
    [SerializeField] private PauseMenuController pauseMenuController;
    [SerializeField] private GameObject optionsPanel;
    
    private const string MasterVolume = "MasterVolume";
    private const string MusicVolume = "MusicVolume";
    private const string SFXVolume = "SFXVolume";
    private const string AmbienceVolume = "AmbienceVolume";
    private const string MouseSensitivity = "MouseSensitivity";
    
    private string currentScene;
    private FirstPersonController playerController;
    
    private void Start()
    {
        // Get current scene name
        currentScene = SceneManager.GetActiveScene().name;
        
        // Try to find the player controller if we're in the game scene
        if (currentScene != "MainMenu")
        {
            playerController = FindObjectOfType<FirstPersonController>();
        }
        
        // Set up button listeners
        backButton.onClick.AddListener(HandleBackButton);
        applyButton.onClick.AddListener(SaveSettings);
        
        // Set up slider listeners
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        ambienceVolumeSlider.onValueChanged.AddListener(SetAmbienceVolume);
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
        ambienceVolumeSlider.value = PlayerPrefs.GetFloat(AmbienceVolume, 0.75f);
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat(MouseSensitivity, 1.0f);
        
        // Apply loaded values
        SetMasterVolume(masterVolumeSlider.value);
        SetMusicVolume(musicVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);
        SetAmbienceVolume(ambienceVolumeSlider.value);
        SetMouseSensitivity(mouseSensitivitySlider.value);
    }
    
    private void SaveSettings()
    {
        // Save settings to PlayerPrefs
        PlayerPrefs.SetFloat(MasterVolume, masterVolumeSlider.value);
        PlayerPrefs.SetFloat(MusicVolume, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(SFXVolume, sfxVolumeSlider.value);
        PlayerPrefs.SetFloat(AmbienceVolume, ambienceVolumeSlider.value);
        PlayerPrefs.SetFloat(MouseSensitivity, mouseSensitivitySlider.value);
        PlayerPrefs.Save();
    }
    
    public void SetMasterVolume(float volume)
    {
        // Convert slider value (0 to 1) to decibels (-80 to 0)
        globalMixer.SetFloat(MasterVolume, Mathf.Log10(volume) * 20);
        
        // Update text display
        if (masterVolumeText != null)
            masterVolumeText.text = (volume * 100).ToString("F0") + "%";
    }
    
    public void SetMusicVolume(float volume)
    {
        globalMixer.SetFloat(MusicVolume, Mathf.Log10(volume) * 20);
        
        // Update text display
        if (musicVolumeText != null)
            musicVolumeText.text = (volume * 100).ToString("F0") + "%";
    }
    
    public void SetSFXVolume(float volume)
    {
        globalMixer.SetFloat(SFXVolume, Mathf.Log10(volume) * 20);
        
        // Update text display
        if (sfxVolumeText != null)
            sfxVolumeText.text = (volume * 100).ToString("F0") + "%";
    }
    
    public void SetAmbienceVolume(float volume)
    {
        globalMixer.SetFloat(AmbienceVolume, Mathf.Log10(volume) * 20);
        
        // Update text display
        if (ambienceVolumeText != null)
            ambienceVolumeText.text = (volume * 100).ToString("F0") + "%";
    }
    
    public void SetMouseSensitivity(float sensitivity)
    {
        // Update text display
        if (sensitivityValueText != null)
            sensitivityValueText.text = sensitivity.ToString("F1");
        
        // Apply to player controller if available
        if (playerController != null)
            playerController.mouseSensitivity = sensitivity;
        
        // If we change scene, the sensitivity will apply from PlayerPrefs
        // Use this in FirstPersonController.Start():
        // mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", mouseSensitivity);
    }
    
    public void HandleBackButton()
    {
        SaveSettings();
        
        // Check which scene we're in and act accordingly
        if (currentScene == "MainMenu")
        {
            // We're in main menu
            if (mainMenuController != null)
                mainMenuController.ShowMainMenu();
        }
        else
        {
            // We're in game scene
            if (pauseMenuController != null)
                pauseMenuController.ReturnToPauseMenu();
            else
                optionsPanel.SetActive(false); // Fallback
        }
    }
}