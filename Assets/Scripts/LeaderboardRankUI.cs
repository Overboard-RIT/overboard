using UnityEngine;
using UnityEngine.Animations;

public class LeaderboardRankUI : MonoBehaviour
{
    public RuntimeAnimatorController[] ranks;
    public Animator rankImage;
    
    public void SetRank(int? rank)
    {
        if (rank == null)
        {
            rankImage.runtimeAnimatorController = null;
            return;
        }

        int nonNullRank = (int)rank - 1;
        if (ranks.Length > nonNullRank)
        {
            rankImage.runtimeAnimatorController = ranks[nonNullRank];
        }
        else
        {
            //Debug.LogError("RankUI: Image component or rank sprite not found at ." + nonNullRank);
        }
    }
}
