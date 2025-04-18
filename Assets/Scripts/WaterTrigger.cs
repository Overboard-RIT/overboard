using UnityEngine;
using System.Collections.Generic;

public class WaterTrigger : MonoBehaviour
{
    public FloatingTextUI penaltyText; // Assign in Inspector
    public float gracePeriod = 0.5f;
    public float penaltyCooldown = 2f; // Time before the player can be penalized again
    public ScoreManager scoreManager; // Assign in Inspector
    public GameManager gameManager; // Assign in Inspector

    private float lastPenaltyTime = -Mathf.Infinity; // Tracks last penalty time

    private float? enteredWaterAt = null;

    public GameObject playerLeftFoot;
    public GameObject playerRightFoot;

    public GameObject splash;

    public GameTimer gameTimer; // Assign in Inspector
    private List<Shark> sharks = new List<Shark>();

    void Update()
    {
        foreach (Shark shark in sharks)
        {
            if (shark != null)
            {
                shark.playerPosition = (playerLeftFoot.transform.position + playerRightFoot.transform.position) / 2;
            }
        }

        if (gameTimer.isGameOver || !gameManager.gameStarted) {
            return;
        }

        FlotsamCollider[] flotsamColliders = FindObjectsByType<FlotsamCollider>(FindObjectsSortMode.None);
        foreach (FlotsamCollider flotsamCollider in flotsamColliders)
        {
            if (flotsamCollider.PlayerContact)
            {
                enteredWaterAt = null;
                foreach (Shark shark in sharks)
                {
                    if (shark != null)
                    {
                        Destroy(shark.gameObject); // Destroy the shark if the player is in contact with flotsam
                    }
                }
                sharks.Clear(); // Clear the list of sharks if the player is in contact with flotsam
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

                Vector3 playerPosition = (playerLeftFoot.transform.position + playerRightFoot.transform.position) / 2;
                playerPosition.y = 0.5f; // Adjust Y position to be above the water
                GameObject shark = Instantiate(splash, playerPosition, Quaternion.Euler(90f, 0f, 0f));
                sharks.Add(shark.GetComponent<Shark>());
            }
        }
    }
}
