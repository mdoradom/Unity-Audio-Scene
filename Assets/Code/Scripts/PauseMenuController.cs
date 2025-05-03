using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause Menu UI")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;
    
    [Header("Game Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    
    private bool isPaused = false;
    
    private void Start()
    {
        // Set up button listeners
        resumeButton.onClick.AddListener(ResumeGame);
        optionsButton.onClick.AddListener(OpenOptions);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        exitButton.onClick.AddListener(ExitGame);
        
        // Initialize
        pauseMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    
    private void Update()
    {
        // Check for pause input (Escape key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
        // Ensure time scale is correct based on pause state
        Time.timeScale = isPaused ? 0f : 1f;
    }
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);
        
        // Set time scale (freeze/unfreeze game)
        Time.timeScale = isPaused ? 0f : 1f;
        
        // Enable/disable cursor for FPS games
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        
        // Hide cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void OpenOptions()
    {
        pauseMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
        // Still paused, keep time scale at 0
        Time.timeScale = 0f;
    }
    
    public void ReturnToMainMenu()
    {
        // Reset time scale before changing scenes
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    public void ReturnToPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        optionsMenuPanel.SetActive(false);
        // Still paused, keep time scale at 0
        Time.timeScale = 0f;
    }
    
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

public class PauseOptionsController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer globalMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider ambienceVolumeSlider;
    
    [Header("Mouse Settings")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;
    
    [Header("UI Elements")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button applyButton;
    
    [Header("References")]
    [SerializeField] private PauseMenuController pauseMenuController;
    
    private const string MasterVolume = "Master";
    private const string MusicVolume = "Music";
    private const string SFXVolume = "SFX";
    private const string AmbienceVolume = "Ambience";
    private const string MouseSensitivity = "MouseSensitivity";
    
    private void Start()
    {
        // Set up button listeners
        backButton.onClick.AddListener(ReturnToPauseMenu);
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
    }
    
    public void SetMusicVolume(float volume)
    {
        globalMixer.SetFloat(MusicVolume, Mathf.Log10(volume) * 20);
    }
    
    public void SetSFXVolume(float volume)
    {
        globalMixer.SetFloat(SFXVolume, Mathf.Log10(volume) * 20);
    }
    
    public void SetAmbienceVolume(float volume)
    {
        globalMixer.SetFloat(AmbienceVolume, Mathf.Log10(volume) * 20);
    }
    
    public void SetMouseSensitivity(float sensitivity)
    {
        if (sensitivityValueText != null)
            sensitivityValueText.text = sensitivity.ToString("F1");
    }
    
    public void ReturnToPauseMenu()
    {
        SaveSettings();
        pauseMenuController.ReturnToPauseMenu();
    }
}