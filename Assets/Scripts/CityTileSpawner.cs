using UnityEngine;
using System.Collections.Generic;

public class CityTileSpawner : MonoBehaviour
{
    public static CityTileSpawner Instance;

    [Header("Tiles")]
    public GameObject startTile;
    public GameObject[] cityTilePrefabs;

    [Header("Settings")]
    public int tilesOnScreen = 6;
    public float tileLength = 30f;

    [Header("Delayed Removal")]
    public int removeAfterTriggers = 5;

    private int triggerCount = 0;
    private float spawnZ = 0f;
    private List<GameObject> activeTiles = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameObject first = Instantiate(startTile, Vector3.forward * spawnZ, Quaternion.identity);
        activeTiles.Add(first);
        spawnZ += tileLength;

        for (int i = 1; i < tilesOnScreen; i++)
        {
            SpawnRandomTile();
        }
    }

    void SpawnRandomTile()
    {
        int randomIndex = Random.Range(0, cityTilePrefabs.Length);

        GameObject tile = Instantiate(
            cityTilePrefabs[randomIndex],
            Vector3.forward * spawnZ,
            Quaternion.identity
        );

        activeTiles.Add(tile);
        spawnZ += tileLength;
    }

    // 🔥 TRIGGER-AWARE + DELAYED REMOVAL
    public void OnPlayerHitTrigger(GameObject currentTile)
    {
        triggerCount++;

        // Always add new tile
        SpawnRandomTile();

        // Remove only after N triggers
        if (triggerCount >= removeAfterTriggers)
        {
            int currentIndex = activeTiles.IndexOf(currentTile);

            if (currentIndex > 0)
            {
                GameObject previousTile = activeTiles[currentIndex - 1];
                activeTiles.RemoveAt(currentIndex - 1);
                Destroy(previousTile);
            }

            triggerCount = 0; // reset counter
        }
    }
}
