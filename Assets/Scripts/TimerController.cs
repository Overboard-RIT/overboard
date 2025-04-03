using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 60.99f;
    public float fadeDuration = 1f;
    public List<PlayerController> players;
    public FlotsamManager spawner;
    public ScoreManager score;
    public BackWallUI backWallUI;
    public Leaderboard leaderboard;
    public CanvasGroup gameOverUI;

    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(timeRemaining, 0); // Ensure time doesn't go below 0

        // Update the timer UI
        backWallUI.SetTimer(Mathf.FloorToInt(timeRemaining));

        // If time reaches zero, trigger game over
        if (timeRemaining <= 0)
        {
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
        // Fade in game-over UI
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            gameOverUI.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameOverUI.alpha = 1;
        gameOverUI.interactable = true;
        gameOverUI.blocksRaycasts = true;
    }

    public void AdjustTime(float amount)
    {
        timeRemaining += amount;
        if (timeRemaining < 0) timeRemaining = 0; // Prevent negative time
    }
}
