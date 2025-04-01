using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public BackWallUI.OverboardPlayer Player
    {
        set
        {
            playerNameText.GetComponent<TextMeshProUGUI>().text = value.name;
            metagameScoreText.GetComponent<TextMeshProUGUI>().text = value.metagameScore.ToString() + "pts";
        }
    }
    public int PlayerNumber
    {
        set
        {
            playerNumberText.GetComponent<TextMeshProUGUI>().text = "P" + value.ToString();
        }
    }
    public GameObject playerNameText;
    public GameObject playerNumberText;
    public GameObject metagameScoreText;
}
