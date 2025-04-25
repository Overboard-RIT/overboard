using UnityEngine;

public class TimerPlatform : MonoBehaviour
{
    public float timerDuration = 1f;
    public GameObject timer;
    public GameObject mask;
    bool timerTicking = false;
    float timerStartedAt = 0f;

    // void Start()
    // {
    //     GetComponent<UIPlatform>().PlatformEntered.AddListener(
    //         () =>
    //         {
    //             if (timerTicking) return;
    //             timerTicking = true;
    //             timerStartedAt = Time.time;
    //             ShowTimer();
    //         }
    //     );
    //     GetComponent<UIPlatform>().PlatformExited.AddListener(
    //         () =>
    //         {
    //             timerTicking = false;
    //             timerStartedAt = 0f;
    //             HideTimer();
    //         }
    //     );
    // }

    void Update()
    {
        if (GetComponent<FlotsamCollider>().PlayerContact)
        {
            if (!timerTicking) {
                timerTicking = true;
                timerStartedAt = Time.time;
                ShowTimer();
            }
        }
        else
        {
            timerTicking = false;
            timerStartedAt = 0f;
            HideTimer();
        }

        if (timerTicking)
        {
            float elapsedTime = Time.time - timerStartedAt;
            SetTimer(elapsedTime);
        }
    }

    public void ShowTimer()
    {
        timer.SetActive(true);
    }

    public void HideTimer()
    {
        timer.SetActive(false);
    }

    public void SetTimer(float time)
    {
        mask.transform.localScale = new Vector3(
            1,
            Mathf.Min(1, time / timerDuration),
            1
        );
    }
}
