using UnityEngine;
using System.Collections;

public class EggSpawner : MonoBehaviour
{
    public GameObject eggPrefab;
    public int eggsToEat = 10;
    public float gridSize = 1f;
    public int gridWidth = 16;
    public int gridHeight = 9;
    [SerializeField] private int eggsEaten = 0;
    private GameObject currentEgg;
    private int gridXMin, gridXMax, gridYMin, gridYMax;
    private bool isProcessingEgg = false;

    void Start()
    {
        eggsEaten = 0;
        isProcessingEgg = false;

        CalculateGridBounds();
        SpawnEgg();

        Debug.Log($"Initial setup: eggsToEat = {eggsToEat}, eggsEaten = {eggsEaten}");
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

        if (currentEgg.tag != "Egg")
        {
            Debug.LogWarning("Egg prefab does not have 'Egg' tag! Setting it now.");
            currentEgg.tag = "Egg";
        }

        MonoBehaviour[] scripts = currentEgg.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != null && script.GetType() != typeof(EggSpawner))
            {
                Debug.LogWarning("Removing script from egg: " + script.GetType().Name);
                Destroy(script);
            }
        }
    }

    public void EggEaten()
    {
        if (isProcessingEgg)
        {
            Debug.Log("Ignoring duplicate EggEaten() call - already processing an egg");
            return;
        }

        isProcessingEgg = true;

        if (currentEgg != null)
        {
            Debug.Log("Processing egg eaten event...");

            Destroy(currentEgg);
            eggsEaten++;
            Debug.Log($"After eating egg: {eggsEaten} / {eggsToEat}");

            StartCoroutine(ProcessNextEgg());
        }
        else
        {
            Debug.LogWarning("EggEaten called but currentEgg is null!");
            isProcessingEgg = false;
        }
    }

    private IEnumerator ProcessNextEgg()
    {
        yield return new WaitForFixedUpdate();

        if (eggsEaten >= eggsToEat)
        {
            Debug.Log("Final egg eaten! Victory achieved!");
            TriggerVictory();
        }
        else
        {
            SpawnEgg();
        }

        isProcessingEgg = false;
    }

    public void TriggerVictory()
    {
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