using UnityEngine;

public class Egg : MonoBehaviour
{
    private EggSpawner spawner;
    private bool hasBeenEaten = false;

    void Start()
    {
        spawner = FindObjectOfType<EggSpawner>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenEaten) return;

        Debug.Log("Egg touched by: " + other.gameObject.name);
        if (other.CompareTag("Chicken"))
        {
            Debug.Log("Chicken ate the egg!");
            hasBeenEaten = true;

            spawner.EggEaten();
        }
    }
}