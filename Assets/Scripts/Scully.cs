using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class Scully : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerPositionDetection playerPositionDetection;
    public float idleTime = 10f;
    public float endGap = 0.5f;
    private ScullyLoader loader;

    public void SquawkIdle()
    {
        loader.PlayScullyLooping(ScullyLoader.ScullyCategory.Idle, idleTime);
    }

    public void SquawkRaft()
    {
        loader.OverrideIdle();
        loader.PlayScullyOnce(ScullyLoader.ScullyCategory.StepOntoTheRaft);
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
        loader.OverrideIdle();
        loader.PlayScullyLooping(ScullyLoader.ScullyCategory.ChooseDifficulty, 6f);
    }

    public void StopOnboarding()
    {
        loader.OverrideIdle();
    }

    public void StartGame()
    {
        loader.OverrideIdle();
    }

    public void SquawkOverboard()
    {
        loader.PlayScullyOnce(ScullyLoader.ScullyCategory.Overboard);
    }

    public void SquawkBanter()
    {
        loader.PlayScullyOnce(ScullyLoader.ScullyCategory.Banter);
    }

    public void SquawkRoundEnd()
    {
        loader.OverrideIdle();
        StartCoroutine(SquawkEndCoroutine());
    }

    private IEnumerator SquawkScoreCoroutine()
    {
        int score = ScoreManager.Instance.Score;
        if (score < 1500)
        {
            yield return StartCoroutine(loader.PlayScullyOnceCoroutine(ScullyLoader.ScullyCategory.LowScore));
        }
        else if (score < 4000)
        {
            yield return StartCoroutine(loader.PlayScullyOnceCoroutine(ScullyLoader.ScullyCategory.NormalScore));
        }
        else
        {
            yield return StartCoroutine(loader.PlayScullyOnceCoroutine(ScullyLoader.ScullyCategory.HighScore));
        }
    }

    private IEnumerator SquawkEndCoroutine()
    {
        // round end
        Debug.Log("Squawk Round End");
        yield return StartCoroutine(loader.PlayScullyOnceCoroutine(ScullyLoader.ScullyCategory.RoundEnd));
        yield return new WaitForSeconds(endGap);

        // score
        Debug.Log("Squawk Score");
        gameManager.GetComponent<VoiceTriggers>().OnShowScore();
        yield return StartCoroutine(SquawkScoreCoroutine());
        yield return new WaitForSeconds(endGap);

        // rank
        Debug.Log("Squawk Rank");
        gameManager.GetComponent<VoiceTriggers>().OnShowRank();
        yield return StartCoroutine(loader.PlayScullyOnceCoroutine(ScullyLoader.ScullyCategory.Rank));
        yield return new WaitForSeconds(endGap);

        // // end game
        // Debug.Log("Squawk End Game");
        // gameManager.GetComponent<VoiceTriggers>().OnGameEnd();
        // yield return StartCoroutine(loader.PlayScullyOnceCoroutine(ScullyLoader.ScullyCategory.EndGame));

        // reload game
        while (playerPositionDetection.GetActivePositions().Count > 0) {
            yield return new WaitForSeconds(0.25f);
        }
        gameManager.ReloadGame();
    }

    void Awake()
    {
        loader = GetComponent<ScullyLoader>();
        if (loader == null)
        {
            Debug.LogError("ScullyLoader component not found on Scully object.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
