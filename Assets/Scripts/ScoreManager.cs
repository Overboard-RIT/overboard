using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton instance
    public BackWallUI scoreUI; // UI Text element to display the score
    // public UnityEngine.UI.Text finalScoreText; // UI Text element to display the score
    [SerializeField]
    private int currentScore = 0;
    public int Score { get { return currentScore; } } // Property to access the score

    void Awake()
    {
        // Ensure only one instance of ScoreManager exists
        if (Instance == null)
        {
            Instance = this;
            enabled = false; // Disable the script until game starts
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        currentScore = 0; // Reset score at the start of the game
        UpdateScoreText();
    }

    public void AddPoints(int points)
    {
        // Add points to the score
        currentScore += points;
        if (currentScore < 0)
        {
            currentScore = 0; // Ensure score doesn't go negative
        }
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        // Update the score text on the UI
        scoreUI.SetScore(currentScore);
        // finalScoreText.text = "Score: " + currentScore;
    }
}