using UnityEngine;

public class DrinkPickupZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerTray tray = other.GetComponent<PlayerTray>();
        if (tray == null)
            return;

        if (DrinkMenuUI.Instance != null)
            DrinkMenuUI.Instance.Open(tray);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (DrinkMenuUI.Instance != null)
            DrinkMenuUI.Instance.Close();
    }
}
