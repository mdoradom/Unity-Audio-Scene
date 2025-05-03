using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Main Menu UI Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI creatorText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    [Header("Game Settings")]
    [SerializeField] private string gameSceneName = "GameScene";
    
    private void Start()
    {
        // Set up button listeners
        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(OpenOptions);
        creditsButton.onClick.AddListener(OpenCredits);
        exitButton.onClick.AddListener(ExitGame);
        
        // Initialize
        ShowMainMenu();

        // Set up cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Time scale should be 1 when starting the game
        Time.timeScale = 1f;
    }
    
    public void PlayGame()
    {
        // Add transition animation here if needed
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
    
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
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