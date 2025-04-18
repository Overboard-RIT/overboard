using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Scully scully;
    public Leaderboard leaderboard;
    public GameTimer gameTimer;
    public ScoreManager scoreManager;
    public WaterTrigger waterTrigger;
    public FlotsamManager flotsamManager;
    public List<StartAndStop> countdownNumbers;
    public StartAndStop start;
    private bool introStarted = false;

    public BackWallUI backWallUI; // Reference to the BackWallUI script
    public float countdownDelay = 1f; // Delay between countdown steps

    public BackgroundAudio backgroundAudio; // Reference to the BackgroundAudio script

    // public bool gameStarted = false;

    [Header("Inspector Controls")]
    public bool startOnboard = false;
    public bool showDifficulty = false;
    public bool startGame = false;
    public bool endGame = false;

    private string playerName;


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

    void Start() {
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

    public void StartCountdown() {
        StartCoroutine(StartGameCountdown());
    }

    private void StartGame()
    {
        // gameStarted = true;
        waterTrigger.enabled = true;
        scoreManager.enabled = true;
        gameTimer.enabled = true;

        gameTimer.timeRemaining = GetComponent<Config>().TimerStartsAt;

        playerName = GetComponent<Names>().GenerateUniquePirateName();
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
        
        backWallUI.Squawk("Go!", "Weigh anchor, and make me rich!");
    }

    public void StartOnboarding()
    {
        flotsamManager.StartOnboarding();
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
        leaderboard.NewScore(playerName, playerName, scoreManager.Score);
        flotsamManager.Stop();
        waterTrigger.enabled = false;
        scoreManager.enabled = false;

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

    public void ReloadGame() {
        GetComponent<VoiceTriggers>().OnIdle();
        backWallUI.GoIdle();
        backgroundAudio.playOnboarding();
        StartOnboarding();

        foreach (GameObject effect in GameObject.FindGameObjectsWithTag("Effect"))
        {
            Destroy(effect);
        }
    }

    private System.Collections.IEnumerator StartGameCountdown()
    {
        backgroundAudio.stopOnboarding();
        backWallUI.Squawk("Ready Yourself, Swabbie!", "");
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
