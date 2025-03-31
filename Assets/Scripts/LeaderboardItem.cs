using UnityEngine;
using TMPro;

public class LeaderboardItem : MonoBehaviour
{
    public GameObject nameText;
    public GameObject scoreText;
    public string Name
    {
        set
        {
            nameText.GetComponent<TextMeshProUGUI>().text = value;
        }
    }
    public int Score
    {
        set
        {
            scoreText.GetComponent<TextMeshProUGUI>().text = value.ToString() + "pts";
        }
    }

    public bool Visible
    {
        set
        {
            nameText.SetActive(value);
            scoreText.SetActive(value);
        }
    }
}
