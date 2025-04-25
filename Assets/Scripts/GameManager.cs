using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Scully scully;
    public Leaderboard leaderboard;
    public GameTimer gameTimer;
    public ScoreManager scoreManager;
    public WaterTrigger waterTrigger;
    public FlotsamManager flotsamManager;
    public List<StartAndStop> countdownNumbers;
    public StartAndStop start;
    private bool introStarted = false;
    public FlotsamManager.Difficulty gameDifficulty;

    public BackWallUI backWallUI; // Reference to the BackWallUI script
    public float countdownDelay = 1f; // Delay between countdown steps
    public BackgroundAudio backgroundAudio; // Reference to the BackgroundAudio script
    public Results results;
    public LeaderboardRankUI leaderboardRank;

    public bool gameStarted = false;
    public MetagameAPI metagameAPI; // Reference to the MetagameAPI script
    public RFIDScanner scanner; // Tell Scanner color

    public int overboards = 0;

    [Header("Inspector Controls")]
    public bool startOnboard = false;
    public bool showDifficulty = false;
    public bool startGame = false;
    public bool endGame = false;

    private string playerName;

    void Awake()
    {
        // Ensure only one instance of ScoreManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnValidate()
    {
        if (endGame)
        {
            endGame = false;
            EndGame();
        }

        if (startOnboard)
        {
            startOnboard = false;
            StartOnboarding();
        }

        if (showDifficulty)
        {
            showDifficulty = false;
            ShowDifficulty();
        }

        if (startGame)
        {
            startGame = false;
            StartCountdown();
        }
    }

    void OnDisable()
    {
        //Debug.LogWarning($"{gameObject.name} was deactivated! Callstack:\n" + Environment.StackTrace);
    }

    void Start()
    {
        ReloadGame();
    }

    void Update()
    {
        // Wait for the spacebar press to start the game
        if (!introStarted && Input.GetKeyDown(KeyCode.Space))
        {
            introStarted = true;
            StartCoroutine(StartGameCountdown());
        }
    }

    public void IncrementOverboards()
    {
        overboards++;
    }

    public void StartCountdown()
    {
        StartCoroutine(StartGameCountdown());
    }

    public void SetPlayerName()
    {
        playerName = GetComponent<Names>().GenerateUniquePirateName("");
        backWallUI.SetPlayerName(playerName);
    }

    private void StartGame()
    {
        waterTrigger.enabled = true;
        scoreManager.enabled = true;
        gameTimer.enabled = true;

        gameTimer.timeRemaining = GetComponent<Config>().TimerStartsAt;
        //Debug.Log(metagameAPI.currentPlayerID);
        playerName = GetComponent<Names>().GenerateUniquePirateName(metagameAPI.currentPlayerID);

        scully.StartGame();
        start.Show();
        backWallUI.AddPlayer(
            new BackWallUI.OverboardPlayer(
                playerName,
                0,
                0 // replace with metagame score
            )
        );
        backWallUI.StartGame();
        flotsamManager.StartSpawning();
        scoreManager.StartGame();
        backgroundAudio.playGameplay();
        GetComponent<VoiceTriggers>().StartBantering();

        // backWallUI.Squawk("Go!", "Weigh anchor, and make me rich!");
    }

    public void StartOnboarding()
    {
        flotsamManager.StartOnboarding();

        // Andrew added this
        // I think it's where you want to trigger this in the UI?
        backWallUI.StartOnboarding();

        // backWallUI.Squawk("Onboarding Text", "Onboarding Text2");
    }

    public void ShowDifficulty()
    {
        flotsamManager.ShowDifficulty();
    }

    public void EndGame()
    {
        // gameStarted = false;
        GetComponent<VoiceTriggers>().OnRoundEnd();
        GetComponent<VoiceTriggers>().StopBantering();

        // fake name until we have a name input
        leaderboardRank.SetRank(leaderboard.GetLeaderboardPosition(playerName, playerName, scoreManager.Score));
        leaderboard.NewScore(playerName, playerName, scoreManager.Score);

        // send score to metagame
        metagameAPI.PostGameData(metagameAPI.currentPlayerID, scoreManager.Score * 100);

        flotsamManager.Stop();
        waterTrigger.enabled = false;
        scoreManager.enabled = false;
        results.gameObject.SetActive(true);
        results.ShowName(playerName);

        foreach (GameObject flotsam in GameObject.FindGameObjectsWithTag("Flotsam"))
        {
            flotsam.GetComponent<FlotsamLifecycle>().EndGame();
        }
        foreach (GameObject coin in GameObject.FindGameObjectsWithTag("Coin"))
        {
            Destroy(coin);
        }
        foreach (GameObject shark in GameObject.FindGameObjectsWithTag("Shark"))
        {
            Destroy(shark);
        }
    }

    public void ReloadGame()
    {
        GetComponent<VoiceTriggers>().OnIdle();
        backWallUI.GoIdle();
        backgroundAudio.playOnboarding();
        StartOnboarding();

        gameStarted = false;
        StartCoroutine(scanner.UpdateLED(RFIDLed.ATTRACT));

        overboards = 0;
        results.Init();


        foreach (GameObject effect in GameObject.FindGameObjectsWithTag("Effect"))
        {
            Destroy(effect);
        }
    }

    private System.Collections.IEnumerator StartGameCountdown()
    {
        backgroundAudio.stopOnboarding();
        backWallUI.Squawk("Ready Yourself, Swabbie!", "");
        gameStarted = true;
        StartCoroutine(scanner.UpdateLED(RFIDLed.OCCUPIED));
        yield return new WaitForSeconds(countdownDelay * 1.5f);

        // Countdown from 3... 2... 1...
        for (int i = 3; i > 0; i--)
        {
            countdownNumbers[i - 1].Show();
            backWallUI.Squawk("Ready Yourself, Swabbie!", "The game will start in " + i.ToString() + "!");
            yield return new WaitForSeconds(countdownDelay);
        }

        StartGame();
        yield return new WaitForSeconds(countdownDelay * 4);
        flotsamManager.startingPlatform.GetComponent<FlotsamLifecycle>().StartGame();

        // Clear the message (optional)
        backWallUI.Quiet();
        // backWallUI.HideScully();
    }


}
