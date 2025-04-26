using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class Results : MonoBehaviour
{
    public GameObject scorePanel;
    public GameObject overboardsPanel;
    public GameObject rankPanel;
    public GameObject leaderboardRankPanel;
    public GameObject namePanel;
    public VideoPlayer scrollPlayer;

    public bool showScore = false;
    public bool showRank = false;

    void OnValidate()
    {
        if (showScore)
        {
            ShowScore();
            ShowOverboards();
            showScore = false;
        }
        if (showRank)
        {
            ShowRank();
            showRank = false;
        }
    }

    public enum Rank
    {
        Captain = 0,
        FirstMate = 1,
        Scallywag = 2,
        Swab = 3,
        WalkThePlank = 4,
    };

    public void Init()
    {
        namePanel.SetActive(false);
        scorePanel.SetActive(false);
        overboardsPanel.SetActive(false);
        rankPanel.SetActive(false);
        leaderboardRankPanel.SetActive(false);

        scorePanel.GetComponent<ValueDisplay>().Value = "0";
        overboardsPanel.GetComponent<ValueDisplay>().Value = "0";

        gameObject.SetActive(false);
    }

    public void ShowName(string name)
    {
        StartCoroutine(ShowNameCoroutine(name));
    }

    private IEnumerator ShowNameCoroutine(string name)
    {
        yield return new WaitForSeconds(0.5f);
        while (scrollPlayer.isPlaying)
        {
            yield return null;
        }
        namePanel.GetComponent<ValueDisplay>().Value = name;
        namePanel.SetActive(true);
    }

    public void ShowScore()
    {
        scorePanel.SetActive(true);
        StartCoroutine(IncreaseNumberTo(
            scorePanel.GetComponent<ValueDisplay>(),
            ScoreManager.Instance.Score,
            3f));
    }

    public void ShowOverboards()
    {
        overboardsPanel.SetActive(true);
        StartCoroutine(IncreaseNumberTo(
            overboardsPanel.GetComponent<ValueDisplay>(),
            GameManager.Instance.overboards,
            1f));
    }

    public void ShowRank()
    {
        int rank = 0;
        if (ScoreManager.Instance.Score >= 5000)
        {
            rank = (int)Rank.Captain;
        }
        else if (ScoreManager.Instance.Score >= 4500)
        {
            rank = (int)Rank.FirstMate;
        }
        else if (ScoreManager.Instance.Score >= 3000)
        {
            rank = (int)Rank.Scallywag;
        }
        else if (ScoreManager.Instance.Score >= 1500)
        {
            rank = (int)Rank.Swab;
        }
        else
        {
            rank = (int)Rank.WalkThePlank;
        }

        rankPanel.GetComponent<RankUI>().SetRank((Rank)rank);
        rankPanel.SetActive(true);
    }

    public void ShowLeaderboardRank()
    {
        leaderboardRankPanel.SetActive(true);
    }

    public IEnumerator IncreaseNumberTo(ValueDisplay display, int targetNumber, float duration)
    {
        float coefficient = targetNumber / Mathf.Pow(duration, 0.1f);
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            display.Value = Mathf.RoundToInt(coefficient * Mathf.Pow(time, 0.1f)).ToString();
            yield return null;
        }
        display.Value = targetNumber.ToString();
    }
}
