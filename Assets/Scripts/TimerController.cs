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
        }
    }

    public void AdjustTime(float amount)
    {
        timeRemaining += amount;
        if (timeRemaining < 0) timeRemaining = 0; // Prevent negative time
    }

}
