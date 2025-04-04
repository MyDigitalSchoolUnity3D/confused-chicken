using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button rejouerButton;
    public Button quitterButton;

    public GameObject victoryPanel;
    public TextMeshProUGUI victoryText;

    public Button rejouerVictoryButton;
    public Button quitterVictoryButton;


    void Start()
    {
        Debug.Log("GameOverManager started");
        gameOverPanel.SetActive(false);

        Time.timeScale = 1;

        rejouerButton.onClick.AddListener(Rejouer);
        quitterButton.onClick.AddListener(Quitter);

        if (victoryPanel != null)
        {
            Debug.Log("Victory panel found and initialized");
            victoryPanel.SetActive(false);

            if (rejouerVictoryButton != null)
                rejouerVictoryButton.onClick.AddListener(Rejouer);

            if (quitterVictoryButton != null)
                quitterVictoryButton.onClick.AddListener(Quitter);
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

    public void Rejouer()
    {
        Debug.Log("CLIC SUR REJOUER");
        ResetGame();
    }

    public void Quitter()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // New method for victory
    public void TriggerVictory()
    {
        Debug.Log("Victory triggered in GameOverManager");
        Time.timeScale = 0; // Pause

        if (victoryPanel != null)
        {
            Debug.Log("Activating victory panel");
            victoryPanel.SetActive(true);

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
        else
        {
            Debug.Log("No victory panel found, creating one");
            CreateVictoryText();
        }

        StartCoroutine(LoadNextSceneAfterDelay());
    }



    // New method to reset static variables and restart the game

    void ResetGame()
    {
        if (ChickenController.chickens != null)
        {
            Debug.Log("Clearing chickens list before scene reload");
            ChickenController.chickens.Clear();
        }

        // Reset time scale
        Time.timeScale = 1;

        SceneManager.LoadScene("scene1");
        Debug.Log("Reset game and reloaded scene");
    }

    private void CreateVictoryText()
    {
        Debug.Log("Creating victory text from scratch");

        GameObject victoryObj = new GameObject("VictoryText");
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas == null)
        {
            Debug.Log("No canvas found, creating one");
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        victoryObj.transform.SetParent(canvas.transform, false);

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

        if (victoryPanel != null)
        {
            shouldCheckInput = shouldCheckInput || victoryPanel.activeSelf;
        }

        if (shouldCheckInput)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Enter key pressed, restarting game");
                ResetGame();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
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


    private IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2f); // Laisse le temps d'afficher la victoire
        Time.timeScale = 1; // Remet le jeu en route
        UnityEngine.SceneManagement.SceneManager.LoadScene("scene2");
    }

}


