using UnityEngine;

public class CityTileTrigger : MonoBehaviour
{
    private bool triggered = false;
    private GameObject parentTile;

    void Start()
    {
        // This trigger belongs to this tile
        parentTile = transform.root.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            CityTileSpawner.Instance.OnPlayerHitTrigger(parentTile);
        }
    }

    // Called when tile is reused or moved
    public void ResetTrigger()
    {
        triggered = false;
    }
}
