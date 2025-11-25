using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Start Panel UI")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private bool startWithButton = true;

    [Header("Money")]
    [SerializeField] private int moneyPerTable = 30;
    public int totalMoney = 0;
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Timer")]
    [SerializeField] private float levelDurationSeconds = 180f;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("End Panel UI")]
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI endTitleText;
    [SerializeField] private TextMeshProUGUI endMoneyText;

    private float timeRemaining;
    private bool timerRunning = true;
    private bool levelEnded = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (startWithButton)
        {
            Time.timeScale = 0f;
            timerRunning = false;
            timeRemaining = levelDurationSeconds;

            if (startPanel != null)
                startPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            timerRunning = true;
            timeRemaining = levelDurationSeconds;

            if (startPanel != null)
                startPanel.SetActive(false);
        }

        UpdateMoneyUI();
        UpdateTimerUI();

        if (endPanel != null)
            endPanel.SetActive(false);
    }

    public void OnClickStart()
    {
        if (levelEnded)
            return;

        if (startPanel != null)
            startPanel.SetActive(false);

        Time.timeScale = 1f;
        timerRunning = true;

        if (timeRemaining <= 0f)
            timeRemaining = levelDurationSeconds;

        UpdateTimerUI();
    }

    public void AddMoneyForTable()
    {
        totalMoney += moneyPerTable;
        UpdateMoneyUI();
    }

    private void Update()
    {
        if (!timerRunning || levelEnded)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            timerRunning = false;
            UpdateTimerUI();
            ShowEndPanel();
        }
        else
        {
            UpdateTimerUI();
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = $"$ {totalMoney}";
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = TimeUtils.FormatSecondsToMMSS(timeRemaining);
    }

    private void ShowEndPanel()
    {
        levelEnded = true;
        Time.timeScale = 0f;

        if (endPanel != null)
            endPanel.SetActive(true);

        if (endTitleText != null)
            endTitleText.text = "LEVEL FINISHED";

        if (endMoneyText != null)
            endMoneyText.text = $"Tips: $ {totalMoney}";
    }
}
