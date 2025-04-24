using UnityEngine;
using System.Collections.Generic;

public class Names : MonoBehaviour
{
    private List<PirateName> pirateNames = new List<PirateName>();

    private struct PirateName : System.IEquatable<PirateName>
    {
        public string firstAdjective;
        // public string secondAdjective;
        public string noun;

        public PirateName(string firstAdjective, string noun)
        {
            this.firstAdjective = firstAdjective;
            // this.secondAdjective = secondAdjective;
            this.noun = noun;
        }

        public override string ToString()
        {
            // return $"{firstAdjective} {secondAdjective} {noun}";
            return $"{firstAdjective}{noun}";
        }

        public static implicit operator string(PirateName pirateName)
        {
            return pirateName.ToString();
        }

        public bool Equals(PirateName other)
        {
            return this.ToString() == other.ToString();
        }
    }

    private string[] adjectives = {
        "Salty",
        "Seafaring",
        "One-Eyed",
        "Tipsy",
        "Ruthless",
        "Rebellious",
        "Daring",
        "Fearsome",
        "Cutthroat",
        "Stormy",
        "Coastal",
        "Nautical",
        "Seasick",
        "Cunning",
        "Scurvy",
        "Soggy",
        "Silly",
        "Splashy",
        "Breezy",
        "Sunny",
        "Bubbly",
        "Foamy",
        "Scaly",
        "Fishy",
        "Brave",
        "Bold",
        "Curious",
        "Crafty",
        "Clever",
        "Greedy",
        "Crabby"
    };

    private string[] nouns = {
        "Scoundrel",
        "Crewmate",
        "Gem",
        "Shark",
        "Seagull",
        "Treasure",
        "Sailor",
        "Beard",
        "Swabbie",
        "Seaweed",
        "Cannon",
        "Mermaid",
        "Beach",
        "Barrel",
        "Matey",
        "Crab",
        "Fish",
        "Starfish",
        "Octopus",
        "Whale",
        "Coral",
        "Island",
        "Pufferfish",
        "Pirate",
        "Buccaneer",
        "Captain",
        "Wave",
        "Tide",
        "Lobster",
        "Anchor",
        "Shrimp"
    };
    public string GenerateUniquePirateName()
    {
        PirateName pirateName = GeneratePirateName();

        // Check if the name is already in the list
        while (pirateNames.Contains(pirateName))
        {
            pirateName = GeneratePirateName();
        }

        // Add the unique name to the list
        pirateNames.Add(pirateName);
        return pirateName;
    }
    private PirateName GeneratePirateName()
    {
        string firstAdjective;
        // string secondAdjective;
        string noun;

        firstAdjective = adjectives[Random.Range(0, adjectives.Length)];
        // secondAdjective = adjectives[Random.Range(0, adjectives.Length)];

        // while (firstAdjective == secondAdjective)
        // {
        //     // Ensure the two adjectives are not the same
        //     secondAdjective = adjectives[Random.Range(0, adjectives.Length)];
        // }

        noun = nouns[Random.Range(0, nouns.Length)];

        return new PirateName(firstAdjective, noun);
    }
}