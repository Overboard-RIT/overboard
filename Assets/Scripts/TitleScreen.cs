using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class TitleScreen : MonoBehaviour
{
    public CanvasGroup instructionsPanel; // Reference to the instructions panel's CanvasGroup
    public UnityEngine.UI.Text blinkingText; // Reference to the blinking "Press Space" text
    public float blinkDuration = 2f; // How long the text should blink before starting the game
    public float blinkInterval = 0.5f; // How fast the text blinks
    public SlideUI slidePanel;
    public bool isGameOver = false;

    private bool gameStarting = false;

    void Update()
    {
        // Only respond to Space if the instructions panel is NOT active
        if (isGameOver)
        {
            if (!gameStarting && instructionsPanel.alpha == 0.01f && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(BlinkTextAndStartGame());
            }
        }
        else
        {
            if (!gameStarting && instructionsPanel.alpha == 0 && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(BlinkTextAndStartGame());
            }
        }
        
    }

    IEnumerator BlinkTextAndStartGame()
    {
        gameStarting = true;

        float timer = 0f;
        bool isVisible = true;

        while (timer < blinkDuration)
        {
            blinkingText.enabled = isVisible;
            isVisible = !isVisible;
            timer += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        // Ensure the text is hidden before starting the game
        blinkingText.enabled = false;

        slidePanel.SlideLeft();

        yield return new WaitForSeconds(0.6f);

        SceneManager.LoadScene("GameScene");
    }
}
