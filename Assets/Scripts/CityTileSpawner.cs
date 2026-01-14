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

    private float spawnZ = 0f;
    private List<GameObject> activeTiles = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Spawn start tile
        GameObject first = Instantiate(
            startTile,
            Vector3.forward * spawnZ,
            Quaternion.identity
        );
        activeTiles.Add(first);
        spawnZ += tileLength;

        // Spawn remaining tiles
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

    // 🔥 CALLED FROM TRIGGER
    public void SlideTileForward()
    {
        // Take the first tile (behind player)
        GameObject tileToMove = activeTiles[0];
        activeTiles.RemoveAt(0);

        // Move it to the end (Z axis only)
        tileToMove.transform.position = new Vector3(
            tileToMove.transform.position.x,
            tileToMove.transform.position.y,
            spawnZ
        );

        // Update next spawn position
        spawnZ += tileLength;

        // Add it back at the end
        activeTiles.Add(tileToMove);
    }
}
