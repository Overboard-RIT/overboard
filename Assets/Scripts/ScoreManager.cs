using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton instance
    public BackWallUI scoreUI; // UI Text element to display the score
    // public UnityEngine.UI.Text finalScoreText; // UI Text element to display the score
    private int currentScore = 0;
    public int Score { get { return currentScore; } } // Property to access the score
    private bool stopWorking = false;

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

    void Start()
    {
        // Update the score UI at the start
        UpdateScoreText();
    }

    public void AddPoints(int points)
    {
        if (!stopWorking)
        {
            // Add points to the score
            currentScore += points;
            UpdateScoreText();
        }
        
    }

    public void Stop()
    {
        stopWorking = true;
    }

    void UpdateScoreText()
    {
        // Update the score text on the UI
        scoreUI.SetScore(currentScore);
        // finalScoreText.text = "Score: " + currentScore;
    }
}