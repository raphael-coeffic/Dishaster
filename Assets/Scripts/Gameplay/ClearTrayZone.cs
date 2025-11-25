using UnityEngine;
using UnityEngine.InputSystem;

public class ClearTrayZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Keyboard.current == null)
            return;

        PlayerTray tray = other.GetComponent<PlayerTray>();
        if (tray == null || tray.IsEmpty())
            return;

        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            tray.ClearTray();
        }
    }
}
