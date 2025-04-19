using UnityEngine;
using UnityEngine.Events;

public class VoiceTriggers : MonoBehaviour
{
    public float banterThreshold = 3f;
    public float banterTimer = 0f;
    public bool bantering = false;

    public UnityEvent TriggerIdle = new UnityEvent();
    public UnityEvent OnboardingStarted = new UnityEvent();
    // public UnityEvent PromptGameBand = new UnityEvent();
    // public UnityEvent HowToPlay = new UnityEvent();
    public UnityEvent PromptDifficulty = new UnityEvent();
    public UnityEvent OnboardingEnded = new UnityEvent();
    public UnityEvent TriggerBanter = new UnityEvent();
    public UnityEvent Overboard = new UnityEvent();
    public UnityEvent RoundEnd = new UnityEvent();
    public UnityEvent ShowScore = new UnityEvent();
    public UnityEvent ShowRank = new UnityEvent();
    public UnityEvent GameEnd = new UnityEvent();


    public void StartBantering()
    {
        bantering = true;
        banterTimer = 0;
    }
    public void StopBantering()
    {
        bantering = false;
        banterTimer = 0;
    }
    public void ResetBanterTimer()
    {
        banterTimer = 0;
    }

    public void Update()
    {
        if (!bantering) return;
        if (banterTimer < banterThreshold)
        {
            banterTimer += Time.deltaTime;
        }
        else
        {
            OnBanter();
            banterTimer = 0;
        }
    }

    public void OnIdle()
    {
        TriggerIdle.Invoke();
    }

    public void OnOnboardingStarted()
    {
        OnboardingStarted.Invoke();
    }

    // public void OnPromptGameBand()
    // {
    //     PromptGameBand.Invoke();
    // }

    // public void OnHowToPlay()
    // {
    //     HowToPlay.Invoke();
    // }

    public void OnPromptDifficulty()
    {
        PromptDifficulty.Invoke();
    }

    public void OnOnboardingEnded()
    {
        OnboardingEnded.Invoke();
    }

    public void OnBanter()
    {
        TriggerBanter.Invoke();
    }

    public void OnOverboard()
    {
        Overboard.Invoke();
    }

    public void OnRoundEnd()
    {
        RoundEnd.Invoke();
    }

    public void OnShowScore()
    {
        ShowScore.Invoke();
    }

    public void OnShowRank()
    {
        ShowRank.Invoke();
    }

    public void OnGameEnd()
    {
        GameEnd.Invoke();
    }
}
