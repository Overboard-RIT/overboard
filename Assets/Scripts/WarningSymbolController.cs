using UnityEngine;

public class WarningSymbolController : MonoBehaviour
{
    public Transform maskTransform; // Reference to the pink sprite renderer
    public float drawSpeed = 1f; // Speed at which the pink sprite is drawn
    public float timeToWait = 1f;
    public SpriteRenderer[] symbolRenderers; // Array of SpriteRenderers for the symbol

    private float waitTimer = 0f; // Timer to track the wait time
    private float drawProgress = 0f; // Progress of the pink sprite being drawn (0 to 1)

    private float dT = 0f;
    private float lastShake = 0f;
    private bool isBlinking = false; // Flag to track if blinking has started
    
    private float blinkInterval = 0.25f; // Interval for blinking (in seconds)
    private float blinkTimer = 0.25f; // Timer for blinking

    private int blinkCount = 0; // Counter for the number of blinks
    private Vector3 initialPosition; // Initial position of the maskTransform

    void Start() {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (waitTimer < timeToWait)
        {
            waitTimer += Time.deltaTime; // Increment the wait timer
            return; // Wait until the specified time has passed
        }

        // Gradually reduce the cutoff value to reveal the pink sprite from bottom to top
        drawProgress += Time.deltaTime * drawSpeed;
        drawProgress = Mathf.Clamp01(drawProgress); // Ensure the value stays between 0 and 1

        // Adjust the scale of the mask to reveal the pink sprite
        Vector3 currentScale = maskTransform.localScale;
        currentScale.y = (1f - drawProgress) * 1.75f; // Scale down the Y-axis
        maskTransform.localScale = currentScale;

        // Gradually increase the Y position of the maskTransform
        Vector3 currentPosition = maskTransform.localPosition;
        currentPosition.y = drawProgress * 1.75f; // Adjust the speed as needed
        maskTransform.localPosition = currentPosition;

        // Start blinking once drawProgress is 1
        if (drawProgress >= 0.8f && !isBlinking)
        {
            isBlinking = true;
        }

        if (isBlinking)
        {
            BlinkSymbol();
        }

        dT += Time.deltaTime; // Increment the delta time
        if (dT > lastShake + 0.04f)
        {
            lastShake = dT;
            
            // Shake the local transform
            if (drawProgress < 0.35f || UnityEngine.Random.value > drawProgress) return;
            transform.position = initialPosition + new Vector3(Random.Range(-0.04f, 0.04f), 8, Random.Range(-0.04f, 0.04f));

        }
        
    }

    private void BlinkSymbol()
    {
        blinkTimer += Time.deltaTime;

        if (blinkTimer >= blinkInterval)
        {
            // Toggle the visibility of all SpriteRenderers
            foreach (var renderer in symbolRenderers)
            {
                renderer.enabled = !renderer.enabled;
            }

            blinkTimer = 0f; // Reset the blink timer
            blinkCount++; // Increment the blink count

            if (blinkCount >= 10) // Stop blinking after 10 blinks
            {
                Destroy(gameObject); // Destroy the GameObject
            }
        }
    }
}