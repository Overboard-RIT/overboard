using UnityEngine;
using System;

public class Stats : MonoBehaviour
{
    public ValueDisplay name;
    public ValueDisplay gameState;
    public ValueDisplay playerVisibility;
    public ValueDisplay score;
    public ValueDisplay timer;
    public ValueDisplay overboardFor;

    public WaterTrigger waterTrigger;
    public GameTimer gameTimer;
    public PlayerPositionDetection playerPositionDetection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ReloadGame();
    }

    public void ReloadGame()
    {
        name.Value = "no player";
        gameState.Value = "Idle";
        playerVisibility.Value = "Out of Range";
        score.Value = "";
        timer.Value = "";
        overboardFor.Value = "";

        name.TextColor = Color.white;
        gameState.TextColor = Color.white;
        playerVisibility.TextColor = Color.red;
        score.TextColor = Color.white;
        timer.TextColor = Color.white;
        overboardFor.TextColor = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        int activePlayers = playerPositionDetection.GetActivePositions().Count;
        playerVisibility.Value = (activePlayers > 0) ? "Visible" : "Out of Range";
        playerVisibility.TextColor = (activePlayers > 0) ? Color.green : Color.red;

        overboardFor.TextColor = Color.green;
        overboardFor.Value = "0";

        if (gameState.Value != "In Game") return;
        if (waterTrigger.enteredWaterAt != null)
        {
            overboardFor.TextColor = Color.red;
            overboardFor.Value = Mathf.Round((Time.time - (float)waterTrigger.enteredWaterAt) * 1000f).ToString();
        }

        timer.TextColor = Color.white;
        if (gameTimer.timeRemaining < 10) timer.TextColor = Color.red;
        timer.Value = Mathf.Round(gameTimer.timeRemaining).ToString();
        score.Value = ScoreManager.Instance.Score.ToString();
    }

    public void SetPlayerName(string playerName)
    {
        name.Value = playerName;
    }

    public void SetGameState(string state)
    {
        gameState.Value = state;
    }
}
