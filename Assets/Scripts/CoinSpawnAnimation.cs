using UnityEngine;

public class CoinSpawnAnimation : MonoBehaviour
{
    public GameObject coinPrefab; // Reference to the coin prefab

    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
        
    }

    // This method will be called when the animation finishes playing
    public void OnAnimationEnd()
    {
        // Instantiate the coin prefab at the current position and rotation
        Instantiate(coinPrefab, transform.position, Quaternion.identity);

        // Start a coroutine to wait before destroying the GameObject
        Destroy(gameObject);
    }
}