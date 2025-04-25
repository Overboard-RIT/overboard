using UnityEngine;

public class Gem : MonoBehaviour
{
    public int points = 300; // Points awarded when the coin is collected
    public GameObject collectAnim;

    void Update()
    {
        // Make the coin spin around the Y-axis (using deltaTime to make it frame-rate independent)
        transform.Rotate(100f * Time.deltaTime, 100f * Time.deltaTime, 0); // 100f is the rotation speed
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Coin Triggered: " + other.name); // Log the name of the object that triggered the collider

        // Check if the player has collided with the coin
        if ((other.CompareTag("LeftFoot") || other.CompareTag("RightFoot")))
        {
            CollectGem();
        }
    }

    void CollectGem()
    {
        // Add points to the score
        ScoreManager.Instance.AddPoints(points);

        Instantiate(collectAnim, transform.position, Quaternion.Euler(90f, 0, 0)); // Instantiate the collection animation at the coin's position
        Destroy(gameObject); // Destroy the coin object
    }
}
