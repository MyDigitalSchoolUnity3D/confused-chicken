using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ChickenController : MonoBehaviour
{
    public float speed = 3f;
    private Vector2 direction = Vector2.right;
    public static List<GameObject> chickens = new List<GameObject>();
    private bool isSafe = true; // New chickens are "safe" for a short time
    private bool isWrapping = false; // Flag to prevent multiple wraps

    void Start()
    {
        if (!chickens.Contains(gameObject))
        {
            chickens.Add(gameObject);
        }

        // If it's not the first chicken, disable collision for a short time
        if (chickens.Count > 1)
        {
            StartCoroutine(EnableCollisionAfterDelay(0.5f));
        }
        else
        {
            isSafe = false; // First chicken should collide immediately
        }
    }

    IEnumerator EnableCollisionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSafe = false;  // Enable self-collision after delay
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        Move();
        CheckScreenWrap();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z))
            ChangeDirection(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            ChangeDirection(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Q))
            ChangeDirection(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            ChangeDirection(Vector2.right);
    }

    void ChangeDirection(Vector2 newDirection)
    {
        if (newDirection != -direction)
        {
            direction = newDirection;
        }
    }

    void Move()
    {
        if (!isWrapping)
        {
            transform.position += (Vector3)direction * speed * Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Egg"))
        {
            // Instead of directly destroying the egg, notify the egg spawner
            EggSpawner eggSpawner = FindObjectOfType<EggSpawner>();
            if (eggSpawner != null)
            {
                eggSpawner.EggEaten();
            }
            else
            {
                // Fallback if no EggSpawner is found
                Destroy(collision.gameObject);
                SpawnNewChicken();
            }

            // Add a new chicken segment regardless
            SpawnNewChicken();
        }
        else if (collision.CompareTag("Chicken") && IsSelfCollision(collision.gameObject))
        {
            if (chickens[0] == gameObject)
            {
                GameOver();
            }
        }

        GameOverManager gameOverManager = Object.FindFirstObjectByType<GameOverManager>();
        if (gameOverManager != null && collision.CompareTag("Mort"))
        {
            gameOverManager.TriggerGameOver();
        }
    }

    void CheckAllEggsCollected()
    {
        // Find all remaining eggs in the scene
        GameObject[] remainingEggs = GameObject.FindGameObjectsWithTag("Egg");

        // Debug the egg count
        Debug.Log("Remaining eggs: " + remainingEggs.Length);

        // If no eggs remain, player has won
        if (remainingEggs.Length == 0)
        {
            Debug.Log("No eggs remaining, triggering victory!");
            VictoryAchieved();
        }
    }

    void VictoryAchieved()
    {
        Debug.Log("Victory! All eggs collected!");

        // Try to find the GameOverManager to display victory message
        GameOverManager gameOverManager = Object.FindFirstObjectByType<GameOverManager>();
        Debug.Log("GameOverManager found: " + (gameOverManager != null));

        if (gameOverManager != null)
        {
            Debug.Log("Calling TriggerVictory on GameOverManager");
            gameOverManager.TriggerVictory();
        }
        else
        {
            Debug.Log("No GameOverManager found, creating direct victory message");
            // If no GameOverManager is available, create a simple victory message
            DisplayVictoryMessage();
        }
    }

    void DisplayVictoryMessage()
    {
        // Create a simple UI text to display victory
        GameObject victoryText = new GameObject("VictoryText");
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

        victoryText.transform.SetParent(canvas.transform, false);

        UnityEngine.UI.Text text = victoryText.AddComponent<UnityEngine.UI.Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.text = "VICTORY! All eggs collected!";
        text.fontSize = 36;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.green;

        RectTransform rect = text.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(600, 100);

        // Pause the game
        Time.timeScale = 0;
    }

    bool IsSelfCollision(GameObject otherChicken)
    {
        int chickenIndex = chickens.IndexOf(otherChicken);
        return chickenIndex >= 5;
    }

    void GameOver()
    {
        Debug.Log("Game Over! The chicken train collided with itself.");
        GameOverManager gameOverManager = Object.FindFirstObjectByType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.TriggerGameOver();
        }
    }

    void CheckScreenWrap()
    {
        if (chickens[0] != gameObject || isWrapping) return; // Only the leader wraps and not if already wrapping

        Vector3 pos = transform.position;
        float screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        float screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float buffer = 0.1f; // Small buffer to prevent getting stuck in a wrapping loop

        if (pos.x < screenLeft)
        {
            StartCoroutine(WrapChickenTrain(screenRight - buffer, pos.y));
        }
        else if (pos.x > screenRight)
        {
            StartCoroutine(WrapChickenTrain(screenLeft + buffer, pos.y));
        }
    }

    IEnumerator WrapChickenTrain(float newX, float newY)
    {
        isWrapping = true;

        // Store the original positions and disable colliders during wrapping
        Vector3[] originalPositions = new Vector3[chickens.Count];
        Collider2D[] colliders = new Collider2D[chickens.Count];

        for (int i = 0; i < chickens.Count; i++)
        {
            originalPositions[i] = chickens[i].transform.position;
            colliders[i] = chickens[i].GetComponent<Collider2D>();
            if (colliders[i] != null)
            {
                colliders[i].enabled = false;
            }
        }

        // Teleport the leader
        chickens[0].transform.position = new Vector3(newX, newY, 0);

        // Explicitly force the physics system to update
        Physics2D.SyncTransforms();

        // Wait for a frame to ensure the teleport happens
        yield return null;

        // Now teleport the followers one by one with a tiny delay
        for (int i = 1; i < chickens.Count; i++)
        {
            // Each follower takes the previous chicken's original position
            Vector3 previousPos = originalPositions[i - 1];
            chickens[i].transform.position = new Vector3(
                previousPos.x + (newX - originalPositions[0].x),
                previousPos.y,
                0
            );
            yield return new WaitForFixedUpdate();
        }

        // Re-enable colliders after a short delay
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < chickens.Count; i++)
        {
            if (colliders[i] != null)
            {
                colliders[i].enabled = true;
            }
        }

        isWrapping = false;
    }

    IEnumerator EnableColliderAfterDelay(GameObject chicken, float delay)
    {
        yield return new WaitForSeconds(delay);

        Collider2D chickenCollider = chicken.GetComponent<Collider2D>();
        if (chickenCollider != null)
        {
            chickenCollider.enabled = true;
        }
    }

    void SpawnNewChicken()
    {
        GameObject lastChicken = chickens[chickens.Count - 1];

        float spacing = 1.0f;

        Vector3 newPosition = lastChicken.transform.position - (Vector3)direction * spacing;
        GameObject newChicken = Instantiate(gameObject, newPosition, Quaternion.identity);

        Destroy(newChicken.GetComponent<ChickenController>());
        newChicken.AddComponent<ChickenFollower>().target = lastChicken.transform;

        Collider2D newChickenCollider = newChicken.GetComponent<Collider2D>();
        if (newChickenCollider != null)
        {
            newChickenCollider.enabled = false;
        }

        chickens.Add(newChicken);
        newChicken.tag = "Chicken";

        StartCoroutine(EnableColliderAfterDelay(newChicken, 0.5f));
    }
}