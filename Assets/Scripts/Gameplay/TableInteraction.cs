using UnityEngine;
using UnityEngine.InputSystem;

public class TableInteraction : MonoBehaviour
{
    [SerializeField] private TableOrder table;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Keyboard.current == null)
            return;

        PlayerTray tray = other.GetComponent<PlayerTray>();
        if (tray == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
            HandleInteraction(tray);
    }

    private void HandleInteraction(PlayerTray tray)
    {
        if (table.state == TableState.Calling)
        {
            if (table.isCallingForClear)
            {
                if (!tray.CanTakeTrash())
                    return;

                tray.TakeTrash(table.guests.Length);
                table.MarkCleared();
                return;
            }

            if (!tray.IsEmpty())
                return;

            table.GenerateOrdersForAllGuests();
        }
        else if (table.state == TableState.WaitingServe)
        {
            table.ServeFromTray(tray);
        }
    }
}
