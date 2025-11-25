using UnityEngine;
using TMPro;

public class BossMessageUI : MonoBehaviour
{
    public static BossMessageUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI bossText;
    [SerializeField] private float defaultDuration = 2f;

    private float timer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (bossText == null)
            return;

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                bossText.text = "";
        }
    }

    public void ShowMessage(string message, Color color, float duration = -1f)
    {
        if (bossText == null)
            return;

        bossText.text = message;
        bossText.color = color;

        timer = (duration > 0f) ? duration : defaultDuration;
    }
}
