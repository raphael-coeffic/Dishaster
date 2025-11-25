using UnityEngine;

public class BossVision : MonoBehaviour
{
    [Header("Message Settings")]
    [SerializeField] private float messageDuration = 2f; // Duration the boss message stays on screen

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerTray tray = other.GetComponent<PlayerTray>();
        if (tray != null)
            ShowBossReaction(tray);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerTray tray = other.GetComponent<PlayerTray>();
        if (tray != null)
        {
            // if the tray state changes (ex: pretending starts/stops),
            // the displayed message updates in real-time.
            ShowBossReaction(tray);
        }
    }

    private void ShowBossReaction(PlayerTray tray)
    {
        if (BossMessageUI.Instance == null)
            return;

        // Tray full → positive reinforcement
        if (!tray.IsEmpty())
        {
            BossMessageUI.Instance.ShowMessage(
                "Good, your tray is full. Keep it up!",
                Color.green,
                messageDuration
            );
        }
        else
        {
            // Tray empty + pretending to work
            if (tray.IsPretending())
            {
                BossMessageUI.Instance.ShowMessage(
                    "Keep working, don't slack off!",
                    Color.yellow,
                    messageDuration
                );
            }
            else
            {
                // Tray empty + not doing anything
                BossMessageUI.Instance.ShowMessage(
                    "Get to work, you're not paid to stand around!",
                    Color.red,
                    messageDuration
                );
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }
}
