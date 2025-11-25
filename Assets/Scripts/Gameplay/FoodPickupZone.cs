using UnityEngine;

public class FoodPickupZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerTray tray = other.GetComponent<PlayerTray>();
        if (tray == null)
            return;

        if (FoodMenuUI.Instance != null)
            FoodMenuUI.Instance.Open(tray);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (FoodMenuUI.Instance != null)
            FoodMenuUI.Instance.Close();
    }
}
