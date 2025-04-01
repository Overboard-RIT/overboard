using UnityEngine;

public class Coin : MonoBehaviour
{
    public int points = 100; // Points awarded when the coin is collected

    void Update()
    {
        // Make the coin spin around the Y-axis (using deltaTime to make it frame-rate independent)
        transform.Rotate(100f * Time.deltaTime, 0, 0); // 100f is the rotation speed
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Coin Triggered: " + other.name); // Log the name of the object that triggered the collider
        
        // Check if the player has collided with the coin
        if ((other.CompareTag("LeftFoot") || other.CompareTag("RightFoot")))
        {
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        // Add points to the score
        ScoreManager.Instance.AddPoints(points);

        gameObject.SetActive(false); // Coin disappears after collection
    }
}
