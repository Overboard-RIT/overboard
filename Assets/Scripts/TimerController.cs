using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;

public class GameTimer : MonoBehaviour
{
    public StartAndStop timeup;
    public float timeRemaining = 60.99f;
    public float fadeDuration = 1f;
    public List<PlayerController> players;
    public FlotsamManager spawner;
    public ScoreManager score;
    public BackWallUI backWallUI;
    public Leaderboard leaderboard;
    public CanvasGroup gameOverUI;

    public AudioSource tenSecondAudio;
    public AudioSource fiveSecondAudio;
    public AudioSource gameOverAudio;

    public bool isGameOver = false;
    public GameManager gameManager;

    void Update()
    {
        if (isGameOver || !gameManager.gameStarted) return;

        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(timeRemaining, 0); // Ensure time doesn't go below 0

        // Update the timer UI
        backWallUI.SetTimer(Mathf.FloorToInt(timeRemaining));

        if (timeRemaining <= 11.05f && timeRemaining > 6 && !tenSecondAudio.isPlaying)
        {
            tenSecondAudio.Play();
        }
        else if (timeRemaining <= 4.0f && timeRemaining > 0 && !fiveSecondAudio.isPlaying)
        {
            tenSecondAudio.Stop();
            fiveSecondAudio.Play();
        }

        // If time reaches zero, trigger game over
        if (timeRemaining <= 0.05f)
        {
            fiveSecondAudio.Stop();
            gameOverAudio.Play();
            isGameOver = true;

            foreach (PlayerController player in players)
            {
                player.enabled = false;
            }

            // fake name until we have a name input
            string fakeName = "Colby" + Random.Range(1, 1000).ToString();
            leaderboard.NewScore(fakeName, fakeName, score.Score);

            spawner.Stop();
            score.Stop();

            StartCoroutine(HandleGameOver());
        }
    }

    IEnumerator HandleGameOver()
    {
        timeup.Show();
        backWallUI.ShowScullyPoint();
        backWallUI.Squawk("Yer Time's Up!", "We hope you had fun, but it's time for someone else to suffer!");
        // Fade in game-over UI
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            //gameOverUI.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameOverUI.alpha = 0.01f;
        gameOverUI.interactable = true;
        gameOverUI.blocksRaycasts = true;
    }

    public void AdjustTime(float amount)
    {
        timeRemaining += amount;
        if (timeRemaining < 0) timeRemaining = 0; // Prevent negative time
    }
}
