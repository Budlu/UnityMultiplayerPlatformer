using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text timerText;
    Coroutine countdown;

    int seconds;

    void Start()
    {
        timerText = GetComponent<Text>();
    }

    private string SecondsToString(int seconds)
    {
        int minutes = seconds / 60;
        int remaining = seconds % 60;

        return string.Format("{0:D2}:{1:D2}", minutes, remaining);
    }

    public void Initialize(int seconds)
    {
        this.seconds = seconds;
        timerText.text = SecondsToString(seconds);
    }

    public void StartCountdown()
    {
        countdown = StartCoroutine(Countdown());
    }

    public void StopCountdown()
    {
        StopCoroutine(countdown);
    }

    IEnumerator Countdown()
    {
        while (seconds > 0)
        {
            yield return new WaitForSeconds(1f);

            --seconds;
            timerText.text = SecondsToString(seconds);
        }

        TimerExpired();
    }

    private void TimerExpired()
    {
        Debug.Log("Timer Expired!");
    }
}
