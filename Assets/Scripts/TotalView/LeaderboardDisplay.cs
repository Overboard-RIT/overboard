using UnityEngine;
using System.Collections.Generic;

public class LeaderboardDisplay : MonoBehaviour
{
    public List<TotalViewLeaderboardEntry> leaderboardEntries;
    public Leaderboard leaderboard;

    public void Start()
    {
        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            leaderboardEntries[i].SetValue(0, "");
        }
    }

    public void RenderLeaderboard()
    {
        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            leaderboardEntries[i].SetValue(0, "");
        }

        List<Leaderboard.LeaderboardEntry> entries = leaderboard.GetLeaderboard();
        for (int i = 0; i < Mathf.Min(entries.Count, leaderboardEntries.Count); i++)
        {
            leaderboardEntries[i].SetValue(entries[i].score, entries[i].playerName);
        }
    }

    public void RemoveEntry(int place)
    {
        leaderboard.RemoveEntry(place);
        RenderLeaderboard();
    }
}
