using UnityEngine;
using TMPro;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Start()
    {
        if (titleText != null)
            titleText.text = "LEVEL FINISHED";

        int gained = GameManager.Instance != null ? GameManager.Instance.totalMoney : 0;

        if (moneyText != null)
            moneyText.text = $"Money earned: $ {gained}";
    }
}
