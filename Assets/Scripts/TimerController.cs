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
    public ScoreManager score;
    public BackWallUI backWallUI;
    public CanvasGroup gameOverUI;
    public GameManager gameManager;

    void Awake()
    {
        enabled = false; // Disable the script until game starts
    }

    void Update()
    {
        // If time reaches zero, trigger game over
        if (timeRemaining <= 0)
        {
            gameManager.EndGame();
            EndGame();
            return; // Do not update if time is up
        }

        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(timeRemaining, 0); // Ensure time doesn't go below 0

        // Update the timer UI
        backWallUI.SetTimer(Mathf.FloorToInt(timeRemaining));
    }

    public void EndGame()
    {
        StartCoroutine(HandleGameOver());
        enabled = false; // Disable the timer
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
