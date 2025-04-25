using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    public AudioSource seagull;
    public AudioSource waves;
    public AudioSource onboarding;
    public AudioSource gameplay;

    public void stopAllAudio()
    {
        seagull.Stop();
        waves.Stop();
        onboarding.Stop();
        gameplay.Stop();
    }

    public void stopOnboarding()
    {
        onboarding.Stop();
    }

    public void playOnboarding()
    {
        gameplay.Stop();
        onboarding.Play();
    }

    public void playGameplay()
    {
        onboarding.Stop();
        gameplay.Play();
    }

    public void playAmbience()
    {
        waves.Play();
        seagull.Play();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playAmbience();
        playOnboarding();
    }
}
