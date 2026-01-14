using UnityEngine;

public class CityTileTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            CityTileSpawner.Instance.SlideTileForward(); // ✅ FIXED
        }
    }
}
