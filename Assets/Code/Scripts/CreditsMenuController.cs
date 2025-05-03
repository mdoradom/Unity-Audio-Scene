using UnityEngine;
using UnityEngine.UI;

public class CreditsMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button backButton;
    
    [Header("References")]
    [SerializeField] private MainMenuController mainMenuController;
    
    private void Start()
    {
        // Set up button listener
        backButton.onClick.AddListener(HandleBackButton);
    }
    
    private void HandleBackButton()
    {
        // Return to main menu
        if (mainMenuController != null)
            mainMenuController.ShowMainMenu();
        else
            Debug.LogWarning("MainMenuController reference not set in CreditsMenuController");
    }
}