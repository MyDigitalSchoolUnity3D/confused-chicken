using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    public GameObject eggPrefab;
    public int eggsToEat = 10;
    public float gridSize = 1f;
    public int gridWidth = 16;
    public int gridHeight = 9;
    private int eggsEaten = 0;
    private GameObject currentEgg;
    private int gridXMin, gridXMax, gridYMin, gridYMax;

    void Start()
    {
        CalculateGridBounds();
        SpawnEgg();
    }

    void CalculateGridBounds()
    {
        gridXMin = 0;
        gridXMax = gridWidth - 1;
        gridYMin = 0;
        gridYMax = gridHeight - 1;
        Debug.Log($"Grid bounds: {gridXMin}, {gridXMax}, {gridYMin}, {gridYMax}");
    }

    void SpawnEgg()
    {
        Debug.Log($"Eggs eaten: {eggsEaten} / {eggsToEat}");

        // Check if we've reached the maximum number of eggs
        if (eggsEaten >= eggsToEat)
        {
            Debug.Log("All eggs have been eaten! Triggering victory!");
            TriggerVictory();
            return;
        }

        int randomGridX = Random.Range(gridXMin, gridXMax);
        int randomGridY = Random.Range(gridYMin, gridYMax);

        Vector2 spawnPosition = new Vector2(randomGridX * gridSize, randomGridY * gridSize);
        Debug.Log($"Egg Position: {spawnPosition}");

        currentEgg = Instantiate(eggPrefab, Vector2.zero, Quaternion.identity);
        currentEgg.transform.localPosition = spawnPosition;

        // Make sure the egg has the correct tag
        if (currentEgg.tag != "Egg")
        {
            Debug.LogWarning("Egg prefab does not have 'Egg' tag! Setting it now.");
            currentEgg.tag = "Egg";
        }
    }

    public void EggEaten()
    {
        if (currentEgg != null)
        {
            Destroy(currentEgg);
            eggsEaten++;
            SpawnEgg(); // This will check if we've reached eggsToEat and trigger victory if needed
        }
    }

    private void TriggerVictory()
    {
        // Find the GameOverManager and call TriggerVictory
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager != null)
        {
            Debug.Log("Calling TriggerVictory on GameOverManager");
            gameOverManager.TriggerVictory();
        }
        else
        {
            Debug.LogError("GameOverManager not found! Cannot trigger victory.");
        }
    }
}