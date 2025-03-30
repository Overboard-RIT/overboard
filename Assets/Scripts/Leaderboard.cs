using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    public int size = 10;
    private LeaderboardList leaderboardList;

    public void Start()
    {
        leaderboardList = new LeaderboardList(size);
    }

    public void NewScore(string id, string playerName, int score)
    {
        LeaderboardEntry entry = new LeaderboardEntry(id, playerName, score);
        leaderboardList.Add(entry);
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
