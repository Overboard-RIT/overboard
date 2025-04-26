using UnityEngine;
using UnityEngine.UI; // Make sure to include this if you're using UI components

public class PenaltyManager : MonoBehaviour
{
    private Animator animator;
    private Image imageComponent; // Reference to the Image component
    public AudioSource splash; // Reference to the AudioSource component

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
        imageComponent = GetComponent<Image>(); // Get the Image component

    }

    // Public method to start the animator
    public void StartAnimation()
    {
        if (animator != null)
        {
            imageComponent.enabled = true; // Enable the Image component
            splash.Play(); // Play the splash sound
            animator.Play("OverboardPenalty", -1, 0f); // Restart the animation
        }
    }

    public void EndAnimation()
    {
        if (animator != null)
        {
            imageComponent.enabled = false; // Disable the Image component
        }
    }
}