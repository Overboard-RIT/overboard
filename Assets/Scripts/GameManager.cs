using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<StartAndStop> countdownNumbers;
    public StartAndStop start;
    private bool introStarted = false;

    public BackWallUI backWallUI; // Reference to the BackWallUI script
    public float countdownDelay = 1f; // Delay between countdown steps

    public BackgroundAudio backgroundAudio; // Reference to the BackgroundAudio script

    public bool gameStarted = false;

    void Update()
    {
        // Wait for the spacebar press to start the game
        if (!introStarted && Input.GetKeyDown(KeyCode.Space))
        {
            introStarted = true;
            StartCoroutine(StartGameCountdown());
        }
    }

    private System.Collections.IEnumerator StartGameCountdown()
    {
        backgroundAudio.stopOnboarding();
        backWallUI.Squawk("Ready Yourself, Swabbie!", "");
        yield return new WaitForSeconds(countdownDelay * 1.5f);

        // Countdown from 3... 2... 1...
        for (int i = 3; i > 0; i--)
        {
            countdownNumbers[i - 1].Show();   
            backWallUI.Squawk("Ready Yourself, Swabbie!", "The game will start in " + i.ToString() + "!");
            yield return new WaitForSeconds(countdownDelay);
        }

        // Start the game
        start.Show();
        backWallUI.ShowScullyPoint();
        gameStarted = true;
        backgroundAudio.playGameplay();
        backWallUI.Squawk("Go!", "Weigh anchor, and make me rich!");
        yield return new WaitForSeconds(countdownDelay * 4);

        // Clear the message (optional)
        backWallUI.Quiet();
        backWallUI.HideScully();
    }


}
