using UnityEngine;

public class CityTileTrigger : MonoBehaviour
{
    private GameObject parentTile;

    void Start()
    {
        parentTile = transform.root.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CityTileSpawner.Instance.OnPlayerHitTrigger(parentTile);
        }
    }
}
