using UnityEngine;
using System.Collections.Generic;

public class CityTileSpawner : MonoBehaviour
{
    public static CityTileSpawner Instance;

    public GameObject cityTilePrefab;
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
        // Pre-spawn tiles so player always sees road
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        GameObject tile = Instantiate(
            cityTilePrefab,
            Vector3.forward * spawnZ,
            Quaternion.identity
        );

        activeTiles.Add(tile);
        spawnZ += tileLength;
    }

    public void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    public void SpawnAndDelete()
    {
        SpawnTile();
        DeleteTile();
    }
}
