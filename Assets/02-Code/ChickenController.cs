using UnityEngine;
using System.Collections.Generic;

public class ChickenController : MonoBehaviour
{
    public float speed = 3f;
    private Vector2 direction = Vector2.right;
    public static List<GameObject> chickens = new List<GameObject>();

    void Start()
    {
        if (!chickens.Contains(gameObject))
        {
            chickens.Add(gameObject);
        }
    }

    void Update()
    {
        HandleInput();
        CheckScreenWrap();
    }

    void FixedUpdate()
    {
        Move();
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
        transform.position += (Vector3)direction * speed * Time.fixedDeltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameOverManager gameOverManager = Object.FindFirstObjectByType<GameOverManager>();
        if (gameOverManager != null)
        {
            if (collision.CompareTag("Mort"))
            {

                gameOverManager.TriggerGameOver();
            }
        }
    }
    void CheckScreenWrap()
    {
        Vector3 pos = transform.position;
        float screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        float screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        if (pos.x < screenLeft)
        {
            pos.x = screenRight;
        }
        else if (pos.x > screenRight)
        {
            pos.x = screenLeft;
        }

        transform.position = pos;
    }

    void SpawnNewChicken()
    {
        GameObject lastChicken = ChickenController.chickens[ChickenController.chickens.Count - 1];

        Vector3 newPosition = lastChicken.transform.position - (Vector3)direction * 0.5f;
        GameObject newChicken = Instantiate(gameObject, newPosition, Quaternion.identity);

        newChicken.AddComponent<ChickenFollower>().target = lastChicken.transform;

        ChickenController.chickens.Add(newChicken);
    }
}
