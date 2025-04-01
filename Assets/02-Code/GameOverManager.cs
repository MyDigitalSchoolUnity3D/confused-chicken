using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    // Add victory panel
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryText;

    void Start()
    {
        Debug.Log("GameOverManager started");
        gameOverPanel.SetActive(false);

        // Initialize victory panel if it exists
        if (victoryPanel != null)
        {
            Debug.Log("Victory panel found and initialized");
            victoryPanel.SetActive(false);
        }
        else
        {
            Debug.Log("No victory panel assigned to GameOverManager");
        }
    }

    public void TriggerGameOver()
    {
        Debug.Log("Game over triggered");
        Time.timeScale = 0; // Pause
        gameOverPanel.SetActive(true);
    }

    // New method for victory
    public void TriggerVictory()
    {
        Debug.Log("Victory triggered in GameOverManager");
        Time.timeScale = 0; // Pause

        // Use victory panel if available
        if (victoryPanel != null)
        {
            Debug.Log("Activating victory panel");
            victoryPanel.SetActive(true);

            // Set victory text if available
            if (victoryText != null)
            {
                Debug.Log("Setting victory text");
                victoryText.text = "VICTORY!\nAll eggs collected!";
            }
            else
            {
                Debug.Log("No victoryText component assigned");
            }
        }
        // If no victory panel, create one
        else
        {
            Debug.Log("No victory panel found, creating one");
            CreateVictoryText();
        }
    }

    // Creates a simple victory text if no victory panel exists
    private void CreateVictoryText()
    {
        Debug.Log("Creating victory text from scratch");

        // Create a simple UI text to display victory
        GameObject victoryObj = new GameObject("VictoryText");
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas == null)
        {
            Debug.Log("No canvas found, creating one");
            // Create canvas if none exists
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        victoryObj.transform.SetParent(canvas.transform, false);

        // Try to use TextMeshPro if available
        bool usedTMP = false;
        try
        {
            TextMeshProUGUI tmpText = victoryObj.AddComponent<TextMeshProUGUI>();
            tmpText.text = "VICTORY!\nAll eggs collected!";
            tmpText.fontSize = 36;
            tmpText.alignment = TextAlignmentOptions.Center;
            tmpText.color = Color.green;
            usedTMP = true;
            Debug.Log("Created TextMeshProUGUI for victory message");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error creating TextMeshProUGUI: " + e.Message);
            usedTMP = false;
        }

        // Fallback to regular Text if TMP fails
        if (!usedTMP)
        {
            UnityEngine.UI.Text text = victoryObj.AddComponent<UnityEngine.UI.Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.text = "VICTORY!\nAll eggs collected!";
            text.fontSize = 36;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.green;
            Debug.Log("Created regular UI.Text for victory message");
        }

        RectTransform rect = victoryObj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(600, 100);

        // Add instructions text
        GameObject instructionsObj = new GameObject("VictoryInstructions");
        instructionsObj.transform.SetParent(canvas.transform, false);

        if (usedTMP)
        {
            TextMeshProUGUI instructionsTmp = instructionsObj.AddComponent<TextMeshProUGUI>();
            instructionsTmp.text = "Press ENTER to play again\nPress ESC to quit";
            instructionsTmp.fontSize = 24;
            instructionsTmp.alignment = TextAlignmentOptions.Center;
            instructionsTmp.color = Color.white;
        }
        else
        {
            UnityEngine.UI.Text instructionsText = instructionsObj.AddComponent<UnityEngine.UI.Text>();
            instructionsText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            instructionsText.text = "Press ENTER to play again\nPress ESC to quit";
            instructionsText.fontSize = 24;
            instructionsText.alignment = TextAnchor.MiddleCenter;
            instructionsText.color = Color.white;
        }

        RectTransform instructionsRect = instructionsObj.GetComponent<RectTransform>();
        instructionsRect.anchoredPosition = new Vector2(0, -80);
        instructionsRect.sizeDelta = new Vector2(600, 80);

        Debug.Log("Victory UI elements created successfully");
    }

    void Update()
    {
        bool shouldCheckInput = gameOverPanel.activeSelf;

        // Also check for victory panel if it exists
        if (victoryPanel != null)
        {
            shouldCheckInput = shouldCheckInput || victoryPanel.activeSelf;
        }

        if (shouldCheckInput)
        {
            if (Input.GetKeyDown(KeyCode.Return)) // Entrée pour rejouer
            {
                Debug.Log("Enter key pressed, restarting game");
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) // Échap pour quitter
            {
                Debug.Log("Escape key pressed, quitting game");
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }
}