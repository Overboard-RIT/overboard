using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddScore : MonoBehaviour
{
    public GameObject nameField;
    public GameObject scoreField;
    public Button submit;
    public Leaderboard leaderboard;
    public LeaderboardDisplay totalViewLeaderboard;

    public void OnValidate()
    {
        submit.interactable = false;
        string name = nameField.GetComponent<TMP_InputField>().text;
        string score = scoreField.GetComponent<TMP_InputField>().text;

        if (name == "") return;
        if (score == "") return;

        int scoreNumber;
        bool success = int.TryParse(score, out scoreNumber);
        if (!success) return;

        submit.interactable = true;
    }

    public void Submit()
    {
        string name = nameField.GetComponent<TMP_InputField>().text;
        string score = scoreField.GetComponent<TMP_InputField>().text;

        int scoreNumber = int.Parse(score);

        leaderboard.NewScore(name, name, scoreNumber);
        totalViewLeaderboard.RenderLeaderboard();

        nameField.GetComponent<TMP_InputField>().text = "";
        scoreField.GetComponent<TMP_InputField>().text = "";
    }
}
