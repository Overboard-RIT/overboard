using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BackWallUI : MonoBehaviour
{
    public GameObject playersPanel;
    public GameObject timer;
    public GameObject score;
    public GameObject player1;
    public GameObject player2;
    public GameObject speechBubble;
    public GameObject speechBubblePanel;
    public GameObject idlePanel;
    private List<OverboardPlayer> players = new List<OverboardPlayer>();

    public GameObject scullyPlatform;
    public GameObject scullyNeutral;
    public GameObject scullyPoint;

    public AudioSource SquawkSound;

    private OverboardPlayer example = new OverboardPlayer("Player 1", 0, 0);

    public struct OverboardPlayer
    {
        public string name;
        public int score;
        public int metagameScore;
        public OverboardPlayer(string name, int score, int metagameScore)
        {
            this.name = name;
            this.score = score;
            this.metagameScore = metagameScore;
        }
    }

    public void SetTimer(int time)
    {
        string timeText;
        if (time == 60)
        {
            timeText = "1:00";
        }
        else
        if (time < 10)
        {
            timeText = "0:0" + time.ToString();
        }
        else
        {
            timeText = "0:" + time.ToString();
        }
        timer.GetComponent<TextMeshProUGUI>().text = timeText;
    }

    public void SetScore(int newScore)
    {
        score.GetComponent<TextMeshProUGUI>().text = newScore.ToString() + " pts";
    }

    public void AddPlayer(OverboardPlayer player)
    {
        players.Add(player);

        if (players.Count == 1)
        {
            GameObject newPlayer = Instantiate(player1, playersPanel.transform);
            newPlayer.GetComponent<PlayerUI>().PlayerNumber = players.Count;
            newPlayer.GetComponent<PlayerUI>().Player = player;
        }
        else if (players.Count == 2)
        {
            GameObject newPlayer = Instantiate(player2, playersPanel.transform);
            newPlayer.GetComponent<PlayerUI>().PlayerNumber = players.Count;
            newPlayer.GetComponent<PlayerUI>().Player = player;
        }
    }

    public void Squawk(string heading, string body)
    {
        foreach (Transform child in speechBubblePanel.transform)
        {
            Destroy(child.gameObject);
        }

        SquawkSound.Play();

        GameObject newBubble = Instantiate(speechBubble, speechBubblePanel.transform);
        newBubble.GetComponent<SpeechBubble>().Heading = heading;
        newBubble.GetComponent<SpeechBubble>().Body = body;
    }

    public void Quiet()
    {
        foreach (Transform child in speechBubblePanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // public void HideScully() {
    //     scullyPlatform.SetActive(false);
    //     scullyNeutral.SetActive(false);
    //     scullyPoint.SetActive(false);
    // }

    // public void ShowScullyPoint() {
    //     scullyPlatform.SetActive(true);
    //     scullyPoint.SetActive(true);
    //     scullyNeutral.SetActive(false);
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        idlePanel.SetActive(false);
        score.GetComponent<TextMeshProUGUI>().text = "0 pts";
        // AddPlayer(example);
        //AddPlayer(example);
        Squawk("Ahoy There!", "Kindly stand upon me trusty raft to start the game!");
    }

    void Start() {
        GoIdle();
    }

    public void GoIdle()
    {
        idlePanel.SetActive(true);
        foreach (Transform child in playersPanel.transform)
        {
            Destroy(child.gameObject);
        }
        players.Clear();
    }
}
