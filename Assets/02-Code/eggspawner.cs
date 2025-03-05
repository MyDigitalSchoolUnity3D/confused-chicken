using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    public GameObject eggPrefab;  // Assign your egg prefab in the inspector
    public int eggsToEat = 10;    // Set the goal
    public Vector2 spawnAreaMin;  // Bottom-left corner of spawn area
    public Vector2 spawnAreaMax;  // Top-right corner of spawn area

    private int eggsEaten = 0;
    private GameObject currentEgg;

    void Start()
    {
        SpawnEgg();
    }

    void SpawnEgg()
    {
        if (eggsEaten >= eggsToEat) return;  // Stop when goal is reached

        Vector2 randomPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        currentEgg = Instantiate(eggPrefab, randomPosition, Quaternion.identity);
    }

    public void EggEaten()
    {
        if (currentEgg != null)
        {
            Destroy(currentEgg);
            eggsEaten++;
            SpawnEgg();
        }
    }
}
