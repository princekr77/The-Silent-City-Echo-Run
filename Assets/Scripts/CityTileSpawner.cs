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
    public int removeAfterTriggers = 5;

    private int triggerCount = 0;
    private float spawnZ = 0f;

    private List<GameObject> activeTiles = new List<GameObject>();
    private Queue<GameObject> tilePool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Start tile
        GameObject first = Instantiate(startTile, Vector3.forward * spawnZ, Quaternion.identity);
        activeTiles.Add(first);
        spawnZ += tileLength;

        // Initial tiles
        for (int i = 1; i < tilesOnScreen; i++)
        {
            GameObject tile = InstantiateRandomTile(spawnZ);
            activeTiles.Add(tile);
            spawnZ += tileLength;
        }
    }

    GameObject InstantiateRandomTile(float zPos)
    {
        int index = Random.Range(0, cityTilePrefabs.Length);
        return Instantiate(cityTilePrefabs[index], Vector3.forward * zPos, Quaternion.identity);
    }

    // 🔥 CALLED FROM TRIGGER
    public void OnPlayerHitTrigger(GameObject currentTile)
    {
        triggerCount++;

        // Always spawn new tile visually
        SpawnOrReuseTile();

        // Delay removal logic
        if (triggerCount < removeAfterTriggers)
            return;

        // Recycle previous tile
        int currentIndex = activeTiles.IndexOf(currentTile);
        if (currentIndex > 0)
        {
            GameObject oldTile = activeTiles[currentIndex - 1];
            activeTiles.RemoveAt(currentIndex - 1);

            oldTile.SetActive(false);
            tilePool.Enqueue(oldTile);
        }
    }

    void SpawnOrReuseTile()
    {
        GameObject tile;

        if (tilePool.Count > 0)
        {
            tile = tilePool.Dequeue();
            tile.SetActive(true);

            // Change tile content
            int index = Random.Range(0, cityTilePrefabs.Length);
            ReplaceTileMesh(tile, cityTilePrefabs[index]);
        }
        else
        {
            tile = InstantiateRandomTile(spawnZ);
        }

        tile.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;

        activeTiles.Add(tile);
    }

    // 🔄 Replace tile visuals safely
    void ReplaceTileMesh(GameObject targetTile, GameObject newPrefab)
    {
        for (int i = targetTile.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(targetTile.transform.GetChild(i).gameObject);
        }

        GameObject newVisual = Instantiate(newPrefab, targetTile.transform);
        newVisual.transform.localPosition = Vector3.zero;
        newVisual.transform.localRotation = Quaternion.identity;
    }
}
