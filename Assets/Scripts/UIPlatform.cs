using UnityEngine;
using UnityEngine.Events;

public class UIPlatform : MonoBehaviour
{
    public UnityEvent PlatformEntered;
    public UnityEvent PlatformExited;
    public bool enterPlatform = false;
    public bool exitPlatform = false;

    public float platformStandDuration = 0.5f;
    private float platformEnteredAt = 0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is on the platform
        if (GetComponent<FlotsamCollider>().PlayerContact)
        {
            // if player has just entered the platform, start the timer
            if (platformEnteredAt == 0)
            {
                platformEnteredAt = Time.time;
            }
            // if player has been on the platform long enough, trigger the event
            if (Time.time - platformEnteredAt > platformStandDuration)
            {
                OnPlatformEntered();
            }
        }
        // if player is not on the platform
        else
        {
            // reset the timer
            platformEnteredAt = 0;
            // if player had been on the platform, trigger the exit event
            if (platformEnteredAt != 0 && Time.time - platformEnteredAt > platformStandDuration)
            {
                OnPlatformExited();
            }
        }
    }
}
