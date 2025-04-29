using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    }
    
    private void Update()
    {
        // Check for pause input (Escape key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
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
        TogglePause();
    }
    
    public void OpenOptions()
    {
        pauseMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
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