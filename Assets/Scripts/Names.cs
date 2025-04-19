using UnityEngine;
using System.Collections.Generic;

public class Names : MonoBehaviour
{
    private List<PirateName> pirateNames = new List<PirateName>();

    private struct PirateName : System.IEquatable<PirateName>
    {
        public string firstAdjective;
        public string secondAdjective;
        public string noun;

        public PirateName(string firstAdjective, string secondAdjective, string noun)
        {
            this.firstAdjective = firstAdjective;
            this.secondAdjective = secondAdjective;
            this.noun = noun;
        }

        public override string ToString()
        {
            return $"{firstAdjective} {secondAdjective} {noun}";
        }

        public static implicit operator string(PirateName pirateName)
        {
            return pirateName.ToString();
        }

        public override Equals(PirateName other)
        {
            return this.ToString() == other.ToString();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(firstAdjective, secondAdjective, noun);
        }
    }

    private string[] adjectives = {
        "Swashbuckling",
        "Salty",
        "Seafaring",
        "One-Eyed",
        "Tipsy",
        "Ruthless",
        "Rebellious",
        "Daring",
        "Fearsome",
        "Treacherous",
        "Cutthroat",
        "Stormy",
        "Coastal",
        "Nautical",
        "Seasick"
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
        "Starfish"
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
    private string GeneratePirateName()
    {
        string firstAdjective;
        string secondAdjective;
        string noun;

        firstAdjective = adjectives[Random.Range(0, adjectives.Length)];
        secondAdjective = adjectives[Random.Range(0, adjectives.Length)];

        while (firstAdjective == secondAdjective)
        {
            // Ensure the two adjectives are not the same
            secondAdjective = adjectives[Random.Range(0, adjectives.Length)];
        }

        noun = nouns[Random.Range(0, nouns.Length)];

        return new PirateName(firstAdjective, secondAdjective, noun);
    }
}