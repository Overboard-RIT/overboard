using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class Scully : MonoBehaviour
{
    public GameManager gameManager;
    public float idleTime = 10f;
    public float endGap = 0.5f;
    private ScullyLoader loader;

    public void SquawkIdle()
    {
        loader.PlayScullyLooping(ScullyLoader.ScullyCategory.Idle, idleTime);
    }

    public void SquawkRaft()
    {
        loader.StopAllCoroutines();
        loader.PlayScully(ScullyLoader.ScullyCategory.StepOntoTheRaft);
    }

    // public void SquawkGameBand()
    // {
    //     loader.PlayScully(ScullyLoader.ScullyCategory.GameBand);
    // }

    // public void SquawkHowToPlay()
    // {
    //     loader.PlayScully(ScullyLoader.ScullyCategory.HowToPlay);
    // }

    public void SquawkChooseDifficulty()
    {
        loader.StopAllCoroutines();
        loader.PlayScullyLooping(ScullyLoader.ScullyCategory.ChooseDifficulty, 1f);
    }

    public void StopOnboarding()
    {
        loader.StopAllCoroutines();
    }

    public void SquawkOverboard()
    {
        if (GetComponent<VideoPlayer>().isPlaying)
        {
            return;
        }
        loader.PlayScully(ScullyLoader.ScullyCategory.Overboard);
    }

    public void SquawkBanter()
    {
        if (GetComponent<VideoPlayer>().isPlaying)
        {
            return;
        }
        loader.PlayScully(ScullyLoader.ScullyCategory.Banter);
    }

    public void SquawkRoundEnd()
    {
        StartCoroutine(SquawkEndCoroutine());
    }

    private IEnumerator SquawkScoreCoroutine()
    {
        int score = ScoreManager.Instance.Score;
        if (score < 1000)
        {
            yield return StartCoroutine(loader.PlayScullyCoroutine(ScullyLoader.ScullyCategory.LowScore));
        }
        else if (score < 2500)
        {
            yield return StartCoroutine(loader.PlayScullyCoroutine(ScullyLoader.ScullyCategory.NormalScore));
        }
        else
        {
            yield return StartCoroutine(loader.PlayScullyCoroutine(ScullyLoader.ScullyCategory.HighScore));
        }
    }

    private IEnumerator SquawkEndCoroutine()
    {
        // round end
        Debug.Log("Squawk Round End");
        yield return StartCoroutine(loader.PlayScullyCoroutine(ScullyLoader.ScullyCategory.RoundEnd));
        yield return new WaitForSeconds(endGap);

        // score
        Debug.Log("Squawk Score");
        gameManager.GetComponent<VoiceTriggers>().OnShowScore();
        yield return StartCoroutine(SquawkScoreCoroutine());
        yield return new WaitForSeconds(endGap);

        // rank
        Debug.Log("Squawk Rank");
        gameManager.GetComponent<VoiceTriggers>().OnShowRank();
        yield return StartCoroutine(loader.PlayScullyCoroutine(ScullyLoader.ScullyCategory.Rank));
        yield return new WaitForSeconds(endGap);

        // end game
        Debug.Log("Squawk End Game");
        gameManager.GetComponent<VoiceTriggers>().OnGameEnd();
        yield return StartCoroutine(loader.PlayScullyCoroutine(ScullyLoader.ScullyCategory.EndGame));

        // reload game
        yield return new WaitForSeconds(5f);
        gameManager.ReloadGame();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loader = GetComponent<ScullyLoader>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
