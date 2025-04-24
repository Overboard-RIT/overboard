using UnityEngine;
using UnityEngine.UI;

public class RankUI : MonoBehaviour
{
    public Sprite[] ranks;
    public Image rankImage;
    
    public void SetRank(Results.Rank rank)
    {;
        if (rankImage != null && ranks.Length > (int)rank)
        {
            rankImage.sprite = ranks[(int)rank];
        }
        else
        {
            Debug.LogError("RankUI: Image component or rank sprite not found.");
        }
    }
}
