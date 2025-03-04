using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class ChickenController : MonoBehaviour
{
    public float speed = 3f;  // Vitesse de la poule
    private Vector2 direction = Vector2.right; // Direction
    private TextMeshProUGUI gameOverText;

    void Start()
    {
        // Trouve l'objet du texte dans la scène
        gameOverText = GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>();
        gameOverText.gameObject.SetActive(false); // Cache le texte au début
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
        if (collision.CompareTag("Mort"))
        {
            Debug.Log("Ah AH AH gros naze t'es mort !");
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        gameOverText.gameObject.SetActive(true); // Affiche le message
        yield return new WaitForSeconds(2f); // Attend 2 secondes
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recharge la scène
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
}