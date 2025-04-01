using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button rejouerButton;
    public Button quitterButton;

    void Start()
    {
        gameOverPanel.SetActive(false);

        rejouerButton.onClick.AddListener(Rejouer);
        quitterButton.onClick.AddListener(Quitter);
    }

    public void TriggerGameOver()
    {
        Time.timeScale = 0; // Pause
        gameOverPanel.SetActive(true);
    }

    public void Rejouer()
    {
        Debug.Log("CLIC SUR REJOUER");
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("scene1");

    }

    public void Quitter()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //     void Update()
    //     {
    //         if (gameOverPanel.activeSelf)
    //         {
    //             if (Input.GetKeyDown(KeyCode.Return)) // Entrée pour rejouer
    //             {
    //                 Time.timeScale = 1;
    //                 SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //             }
    //             else if (Input.GetKeyDown(KeyCode.Escape)) // Échap pour quitter
    //             {
    //                 Application.Quit();

    // #if UNITY_EDITOR
    //                 UnityEditor.EditorApplication.isPlaying = false;
    // #endif
    //             }
    //         }
    //     }
}
