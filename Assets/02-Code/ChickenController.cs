using UnityEngine;

public class ChickenController : MonoBehaviour
{
    public float speed = 5f;  // Vitesse de la poule
    private Vector2 direction = Vector2.right; // Direction initiale

    void Update()
    {
        HandleInput();
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
        // Empêche de faire demi-tour sur soi-même
        if (newDirection != -direction)
        {
            direction = newDirection;
        }
    }

    void Move()
    {
        transform.position += (Vector3)direction * speed * Time.fixedDeltaTime;
    }
}
