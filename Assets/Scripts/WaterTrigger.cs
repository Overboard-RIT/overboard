using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public FloatingTextUI penaltyText; // Assign in Inspector
    public float gracePeriod = 0.5f;
    public float penaltyCooldown = 2f; // Time before the player can be penalized again
    public ScoreManager scoreManager; // Assign in Inspector
    public GameManager gameManager; // Assign in Inspector

    private float lastPenaltyTime = -Mathf.Infinity; // Tracks last penalty time

    private float? enteredWaterAt = null;

    public GameTimer gameTimer; // Assign in Inspector

    void Update()
    {
        if (gameTimer.isGameOver || !gameManager.gameStarted) {
            return;
        }

        FlotsamCollider[] flotsamColliders = FindObjectsByType<FlotsamCollider>(FindObjectsSortMode.None);
        foreach (FlotsamCollider flotsamCollider in flotsamColliders)
        {
            if (flotsamCollider.PlayerContact)
            {
                enteredWaterAt = null;
                return;
            }
        }

        if (enteredWaterAt == null && Time.time >= lastPenaltyTime + penaltyCooldown)
        {
            enteredWaterAt = Time.time;
        }
        else
        {
            if (Time.time - enteredWaterAt >= gracePeriod && // Check if grace period has elapsed
            Time.time >= lastPenaltyTime + penaltyCooldown) // Check if enough time has passed since the last penalty
            {
                scoreManager.AddPoints(-200);
                penaltyText.ShowText(); // Show "-5 SECONDS!" text
                lastPenaltyTime = Time.time; // Update penalty timer
                enteredWaterAt = null;
            }
        }
    }
}
