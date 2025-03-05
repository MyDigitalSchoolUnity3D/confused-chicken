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

    }

    void SpawnEgg()
    {
        if (eggsEaten >= eggsToEat) return;

        int randomGridX = Random.Range(gridXMin, gridXMax);
        int randomGridY = Random.Range(gridYMin, gridYMax);

        Vector2 spawnPosition = new Vector2(randomGridX * gridSize, randomGridY * gridSize);

        Debug.Log($" Egg Position: {spawnPosition}");

        currentEgg = Instantiate(eggPrefab, spawnPosition, Quaternion.identity);
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
