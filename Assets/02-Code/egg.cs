using UnityEngine;

public class Egg : MonoBehaviour
{
    private EggSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<EggSpawner>();  // Find the spawner in the scene
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Egg touched by: " + other.gameObject.name); // Check which object is colliding

        if (other.CompareTag("Chicken")) // Ensure your snake has the tag "Snake"
        {
            Debug.Log("Chicken ate the egg!");
            spawner.EggEaten();
            Destroy(gameObject);  // Remove the egg
        }
    }
}
