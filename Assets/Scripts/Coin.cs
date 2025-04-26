using UnityEngine;

public class Coin : MonoBehaviour
{
    public int points = 200; // Points awarded when the coin is collected
    public GameObject collectAnim;

    void Update()
    {
        // Make the coin spin around the Y-axis (using deltaTime to make it frame-rate independent)
        transform.Rotate(100f * Time.deltaTime, 0, 0); // 100f is the rotation speed
    }

    void OnTriggerEnter(Collider other)
    {
        
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

        Instantiate(collectAnim, transform.position, Quaternion.Euler(90f, 0, 0)); // Instantiate the collection animation at the coin's position

        Destroy(gameObject); // Destroy the coin object
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(1f, 0f, 1f, 0.25f);
        float radius = GetComponent<SphereCollider>().radius;
        Vector3 center = GetComponent<SphereCollider>().center;
        Gizmos.DrawSphere(center, radius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(center, radius);
    }
}
