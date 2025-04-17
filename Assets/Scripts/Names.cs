using UnityEngine;
using System.Collections.Generic;

public class Names : MonoBehaviour
{
    private List<string> pirateNames = new List<string>();

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
        "Matey"
    };
    public string GenerateUniquePirateName()
    {
        string pirateName = GeneratePirateName();

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

        return firstAdjective + " " + secondAdjective + " " + noun;
    }
}