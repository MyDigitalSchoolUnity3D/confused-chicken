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
        gameOverPanel.SetActive(false);

        // Initialize victory panel if it exists
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void TriggerGameOver()
    {
        Time.timeScale = 0; // Pause
        gameOverPanel.SetActive(true);
    }

    // New method for victory
    public void TriggerVictory()
    {
        Time.timeScale = 0; // Pause

        // Use victory panel if available
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);

            // Set victory text if available
            if (victoryText != null)
            {
                victoryText.text = "VICTORY!\nAll eggs collected!";
            }
        }
        // If no victory panel, create one
        else
        {
            CreateVictoryText();
        }
    }

    // Creates a simple victory text if no victory panel exists
    private void CreateVictoryText()
    {
        // Create a simple UI text to display victory
        GameObject victoryObj = new GameObject("VictoryText");
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas == null)
        {
            // Create canvas if none exists
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        victoryObj.transform.SetParent(canvas.transform, false);

        // Add TextMeshPro component if available, fallback to regular Text if not
        TextMeshProUGUI tmpText = null;

        try
        {
            tmpText = victoryObj.AddComponent<TextMeshProUGUI>();
            tmpText.text = "VICTORY!\nAll eggs collected!";
            tmpText.fontSize = 36;
            tmpText.alignment = TextAlignmentOptions.Center;
            tmpText.color = Color.green;
        }
        catch
        {
            // Fallback to regular UI.Text
            UnityEngine.UI.Text text = victoryObj.AddComponent<UnityEngine.UI.Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.text = "VICTORY!\nAll eggs collected!";
            text.fontSize = 36;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.green;
        }

        RectTransform rect = victoryObj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(600, 100);

        // Add instructions text
        GameObject instructionsObj = new GameObject("VictoryInstructions");
        instructionsObj.transform.SetParent(canvas.transform, false);

        if (tmpText != null)
        {
            TextMeshProUGUI instructionsTmp = instructionsObj.AddComponent<TextMeshProUGUI>();
            instructionsTmp.text = "Press ENTER to play again\nPress ESC to quit";
            instructionsTmp.fontSize = 24;
            instructionsTmp.alignment = TextAlignmentOptions.Center;
            instructionsTmp.color = Color.white;

            RectTransform instructionsRect = instructionsObj.GetComponent<RectTransform>();
            instructionsRect.anchoredPosition = new Vector2(0, -80);
            instructionsRect.sizeDelta = new Vector2(600, 80);
        }
        else
        {
            UnityEngine.UI.Text instructionsText = instructionsObj.AddComponent<UnityEngine.UI.Text>();
            instructionsText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            instructionsText.text = "Press ENTER to play again\nPress ESC to quit";
            instructionsText.fontSize = 24;
            instructionsText.alignment = TextAnchor.MiddleCenter;
            instructionsText.color = Color.white;

            RectTransform instructionsRect = instructionsObj.GetComponent<RectTransform>();
            instructionsRect.anchoredPosition = new Vector2(0, -80);
            instructionsRect.sizeDelta = new Vector2(600, 80);
        }
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
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (Input.GetKeyDown(KeyCode.Escape)) // Échap pour quitter
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }
}