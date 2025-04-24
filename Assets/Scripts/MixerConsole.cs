using UnityEngine;

public class MixerConsole : MonoBehaviour
{
    [Header("Channels")]

    public AudioSource channel1;
    [SerializeField, Range(0f, 1f)] private float channel1Level;

    public AudioSource channel2;
    [SerializeField, Range(0f, 1f)] private float channel2Level;

    public AudioSource channel3;
    [SerializeField, Range(0f, 1f)] private float channel3Level;

    public AudioSource channel4;
    [SerializeField, Range(0f, 1f)] private float channel4Level;

    public AudioSource channel5;
    [SerializeField, Range(0f, 1f)] private float channel5Level;

    public AudioSource channel6;
    [SerializeField, Range(0f, 1f)] private float channel6Level;

    public AudioSource channel7;
    [SerializeField, Range(0f, 1f)] private float channel7Level;

    public AudioSource channel8;
    [SerializeField, Range(0f, 1f)] private float channel8Level;

    // New channels
    public AudioSource channel9;
    [SerializeField, Range(0f, 1f)] private float channel9Level;

    public AudioSource channel10;
    [SerializeField, Range(0f, 1f)] private float channel10Level;

    public AudioSource channel11;
    [SerializeField, Range(0f, 1f)] private float channel11Level;

    public AudioSource channel12;
    [SerializeField, Range(0f, 1f)] private float channel12Level;

    public AudioSource channel13;
    [SerializeField, Range(0f, 1f)] private float channel13Level;

    public AudioSource channel14;
    [SerializeField, Range(0f, 1f)] private float channel14Level;

    public AudioSource channel15;
    [SerializeField, Range(0f, 1f)] private float channel15Level;

    public AudioSource channel16;
    [SerializeField, Range(0f, 1f)] private float channel16Level;

    private void OnValidate()
    {
        if (channel1 != null) channel1.volume = channel1Level;
        if (channel2 != null) channel2.volume = channel2Level;
        if (channel3 != null) channel3.volume = channel3Level;
        if (channel4 != null) channel4.volume = channel4Level;
        if (channel5 != null) channel5.volume = channel5Level;
        if (channel6 != null) channel6.volume = channel6Level;
        if (channel7 != null) channel7.volume = channel7Level;
        if (channel8 != null) channel8.volume = channel8Level;

        // New channels
        if (channel9 != null) channel9.volume = channel9Level;
        if (channel10 != null) channel10.volume = channel10Level;
        if (channel11 != null) channel11.volume = channel11Level;
        if (channel12 != null) channel12.volume = channel12Level;
        if (channel13 != null) channel13.volume = channel13Level;
        if (channel14 != null) channel14.volume = channel14Level;
        if (channel15 != null) channel15.volume = channel15Level;
        if (channel16 != null) channel16.volume = channel16Level;
    }

    void Start()
    {
        OnValidate();
    }
}
