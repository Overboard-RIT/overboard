using UnityEngine;

public class TotalViewLeaderboardEntry : MonoBehaviour
{
    public ValueDisplay scoreDisplay;
    public ValueDisplay nameDisplay;

    public void SetValue(int score, string name)
    {
        scoreDisplay.Value = score.ToString() + "pts";
        nameDisplay.Value = name;
    }
}