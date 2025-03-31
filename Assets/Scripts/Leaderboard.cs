using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    public List<LeaderboardItem> leaderboardItems;
    private LeaderboardList leaderboardList;
    private LeaderboardEntry test = new LeaderboardEntry("test", "Test Player", 100);
    private LeaderboardEntry test2 = new LeaderboardEntry("test2", "Test Player 2", 500);
    private LeaderboardEntry test3 = new LeaderboardEntry("test3", "Test Player 3", 500);
    private LeaderboardEntry test4 = new LeaderboardEntry("test4", "Test Player 4", 150);
    private LeaderboardEntry test5 = new LeaderboardEntry("test5", "Test Player 5", 250);
    private LeaderboardEntry test6 = new LeaderboardEntry("test6", "Test Player 6", 300);
    private LeaderboardEntry test7 = new LeaderboardEntry("test7", "Test Player 7", 400);
    private LeaderboardEntry test8 = new LeaderboardEntry("test8", "Test Player 8", 350);
    private LeaderboardEntry test9 = new LeaderboardEntry("test9", "Test Player 9", 450);
    private LeaderboardEntry test10 = new LeaderboardEntry("test10", "Test Player 10", 500);


    public void Start()
    {
        leaderboardList = new LeaderboardList(leaderboardItems.Count);
        NewScore(test.id, test.playerName, test.score);
        NewScore(test2.id, test2.playerName, test2.score);
        NewScore(test3.id, test3.playerName, test3.score);
        NewScore(test4.id, test4.playerName, test4.score);
        NewScore(test5.id, test5.playerName, test5.score);
        NewScore(test6.id, test6.playerName, test6.score);
        NewScore(test7.id, test7.playerName, test7.score);
        NewScore(test8.id, test8.playerName, test8.score);
        NewScore(test9.id, test9.playerName, test9.score);
        NewScore(test10.id, test10.playerName, test10.score);
    }

    public void NewScore(string id, string playerName, int score)
    {
        LeaderboardEntry entry = new LeaderboardEntry(id, playerName, score);
        leaderboardList.Add(entry);

        // update the leaderboard UI
        for (int i = 0; i < leaderboardItems.Count; i++)
        {
            if (i < leaderboardList.Count)
            {
                leaderboardItems[i].Name = leaderboardList[i].playerName;
                leaderboardItems[i].Score = leaderboardList[i].score;
                leaderboardItems[i].Visible = true;
            }
            else
            {
                leaderboardItems[i].Visible = false;
            }
        }
    }

    public List<LeaderboardEntry> GetLeaderboard()
    {
        return leaderboardList.Entries;
    }

    public struct LeaderboardEntry
    {
        public LeaderboardEntry(string id, string playerName, int score)
        {
            this.id = id;
            this.playerName = playerName;
            this.score = score;
        }
        public string id;
        public string playerName;
        public int score;
    }

    public class LeaderboardList : IList<LeaderboardEntry>
    {
        // entries
        private List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
        public List<LeaderboardEntry> Entries { get => entries; }
        private int maxEntries;
        public int Count => entries.Count;
        public bool IsReadOnly => false;

        public LeaderboardList(int maxEntries)
        {
            this.maxEntries = maxEntries;
        }

        public LeaderboardEntry this[int index]
        {
            get => entries[index];
            set => throw new System.NotSupportedException("Set is not supported");
        }
        public void Add(LeaderboardEntry entry)
        {
            // if the leaderboard is full and the new entry is lower than the lowest score, return out
            if (entries.Count >= maxEntries && entry.score < entries.Last().score)
                return;
            // if player is already on the leaderboard, keep the higher score
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].id == entry.id)
                {
                    if (entry.score > entries[i].score)
                    {
                        entries[i] = entry;
                        entries.Sort((e1, e2) => e2.score.CompareTo(e1.score));
                        return;
                    }
                }
            }
            // if the leaderboard is not full, add the entry
            if (entries.Count < maxEntries)
            {
                entries.Add(entry);
                entries.Sort((e1, e2) => e2.score.CompareTo(e1.score));
                return;
            }
            // leaderboard is full, player not already on, see if player made the leaderboard
            if (entry.score < entries.Last().score) return;
            entries.Add(entry);
            entries.Sort((e1, e2) => e2.score.CompareTo(e1.score));
            return;
        }
        public void Clear() => entries.Clear();
        public bool Contains(LeaderboardEntry item) => entries.Contains(item);
        public void CopyTo(LeaderboardEntry[] array, int arrayIndex) => entries.CopyTo(array, arrayIndex);
        public IEnumerator<LeaderboardEntry> GetEnumerator() => entries.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int IndexOf(LeaderboardEntry item) => entries.IndexOf(item);
        public void Insert(int index, LeaderboardEntry item)
        {
            throw new System.NotSupportedException("Insert is not supported");
        }
        public bool Remove(LeaderboardEntry item) => false; // no removal
        public void RemoveAt(int index)
        {
            throw new System.NotSupportedException("RemoveAt is not supported");
        }
    }
}
