using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class LeaderboardRankUI : MonoBehaviour
{
    public RuntimeAnimatorController[] ranks;
    public Animator rankImage;
    
    public void SetRank(int? rank)
    {
        if (rank == null)
        {
            rankImage.runtimeAnimatorController = null;
            GetComponent<Image>().color = Color.clear;
            rankImage.GetComponent<Image>().color = Color.clear;
            return;
        }

        int nonNullRank = (int)rank;
        if (nonNullRank < ranks.Length)
        {
            GetComponent<Image>().color = Color.white;
            rankImage.GetComponent<Image>().color = Color.white;
            rankImage.runtimeAnimatorController = ranks[nonNullRank];
        }
        else
        {
            rankImage.runtimeAnimatorController = null;
            GetComponent<Image>().color = Color.clear;
            rankImage.GetComponent<Image>().color = Color.clear;
            //Debug.LogError("RankUI: Image component or rank sprite not found at ." + nonNullRank);
        }
    }
}
