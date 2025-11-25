using System.Collections;
using UnityEngine;

public class PlayerBlinkWhenPretend : MonoBehaviour
{
    [SerializeField] private PlayerTray tray;
    [SerializeField] private Renderer[] renderersToBlink;
    [SerializeField] private float blinkInterval = 0.2f;

    private Coroutine blinkRoutine;

    private void Update()
    {
        if (tray == null)
            return;

        bool pretending = tray.IsPretending();

        if (pretending && blinkRoutine == null)
        {
            blinkRoutine = StartCoroutine(Blink());
        }
        else if (!pretending && blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
            SetVisible(true);
        }
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            ToggleVisible();
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void ToggleVisible()
    {
        foreach (var r in renderersToBlink)
            if (r != null)
                r.enabled = !r.enabled;
    }

    private void SetVisible(bool value)
    {
        foreach (var r in renderersToBlink)
            if (r != null)
                r.enabled = value;
    }
}
