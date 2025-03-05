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

        Debug.Log($"{gridXMin} {gridYMax} {gridYMin} {gridXMax}");


    }

    void SpawnEgg()
    {
        if (eggsEaten >= eggsToEat) return;

        int randomGridX = Random.Range(gridXMin, gridXMax);
        int randomGridY = Random.Range(gridYMin, gridYMax);

        // var newEgg = Instantiate(eggPrefab, Vector3.zero, Quaternion.identity);
        // var newEgg2 = Instantiate(eggPrefab, Vector3.zero, Quaternion.identity);
        // var newEgg3 = Instantiate(eggPrefab, Vector3.zero, Quaternion.identity);
        // var newEgg4 = Instantiate(eggPrefab, Vector3.zero, Quaternion.identity);

        // newEgg.transform.position = new Vector2(gridXMin, gridYMax);
        // newEgg2.transform.position = new Vector2(gridXMax, gridYMin);
        // newEgg3.transform.position = new Vector2(gridXMax, gridYMax);
        // newEgg4.transform.position = new Vector2(gridXMin, gridYMin);


        Vector2 spawnPosition = new Vector2(randomGridX * gridSize, randomGridY * gridSize);

        Debug.Log($" Egg Position: {spawnPosition}");

        currentEgg = Instantiate(eggPrefab, Vector2.zero, Quaternion.identity);
        currentEgg.transform.localPosition = spawnPosition;
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
