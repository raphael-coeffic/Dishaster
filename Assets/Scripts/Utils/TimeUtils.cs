using UnityEngine;

public static class TimeUtils
{
    public static string FormatSecondsToMMSS(float seconds)
    {
        float clamped = Mathf.Max(0f, seconds);

        int t = Mathf.CeilToInt(clamped);
        int minutes = t / 60;
        int sec = t % 60;

        return $"{minutes:00}:{sec:00}";
    }
}
