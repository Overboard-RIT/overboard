using UnityEngine;
using UnityEngine.Events;

public class UIPlatform : MonoBehaviour
{
    public UnityEvent PlatformEntered = new UnityEvent();
    public UnityEvent PlatformExited = new UnityEvent();
    public bool enterPlatform = false;
    public bool exitPlatform = false;
    public float platformStandDuration = 0.5f;
    public float enterTimerStartedAt = 0f;
    public float exitTimerStartedAt = 0f;
    public bool platformEntered = false;

    void OnValidate()
    {
        if (enterPlatform)
        {
            enterPlatform = false;
            OnPlatformEntered();
        }
        if (exitPlatform)
        {
            exitPlatform = false;
            OnPlatformExited();
        }
    }

    protected virtual void OnPlatformEntered()
    {
        PlatformEntered?.Invoke();
    }
    protected virtual void OnPlatformExited()
    {
        PlatformExited?.Invoke();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlatformEntered == null)
        {
            PlatformEntered = new UnityEvent();
        }
        if (PlatformExited == null)
        {
            PlatformExited = new UnityEvent();
        }
        if (GetComponent<TimerPlatform>() != null)
        {
            GetComponent<TimerPlatform>().timerDuration = platformStandDuration;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is on the platform
        if (GetComponent<FlotsamCollider>().PlayerContact)
        {
            exitTimerStartedAt = 0;

            // if player has just entered the platform, start the timer
            if (!platformEntered && enterTimerStartedAt == 0)
            {
                enterTimerStartedAt = Time.time;
            }
            // if player has been on the platform long enough, trigger the event
            if (Time.time - enterTimerStartedAt > platformStandDuration && !platformEntered)
            {
                platformEntered = true;
                OnPlatformEntered();
            }
        }
        // if player is not on the platform
        else
        {
            enterTimerStartedAt = 0;

            // if player has just entered the platform, start the timer
            if (platformEntered && exitTimerStartedAt == 0)
            {
                exitTimerStartedAt = Time.time;
            }
            // if player has been on the platform long enough, trigger the event
            if (Time.time - exitTimerStartedAt > platformStandDuration && platformEntered)
            {
                platformEntered = false;
                OnPlatformExited();
            }
        }
    }
}
