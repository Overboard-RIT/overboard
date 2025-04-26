using UnityEngine;
using System.Collections.Generic;

public class Names : MonoBehaviour
{
    private static List<PirateName> pirateNames;
    private static Dictionary<string, PirateName> pirateNameDictionary;

    public struct PirateName : System.IEquatable<PirateName>
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

        public string WithSpaces() {
            return $"{firstAdjective} {noun}";
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

    private static string[] adjectives = {
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
        "Silly",
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
        "Crabby",
        "Rusty",
        "Risky",
        "Calm",
        "Intrepid",
        "Reckless",
        "Legendary",
        "Mysterious",
        "Wily",
        "Witty",
        "Whimsical",
        "Devious",
        "Rippling",
        "Buoyant",
        "Gallant",
        "Dashing",
        "Tidal",
        "Colossal",
        "Vast",
        "Sunburnt"
    };

    private static string[] nouns = {
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
        "Shrimp",
        "Harpoon",
        "Compass",
        "Buoy",
        "Plank",
        "Rudder",
        "Helm",
        "Cutlass",
        "Parrot",
        "Pegleg",
        "Galleon",
        "Brig",
        "Kraken",
        "Barnacle",
        "Mariner",
        "Port",
        "Stern",
        "Scallywag",
        "Galley",
        "Seashell",
        "Navigator",
        "Lookout",
        "Shipwreck",
        "Sandbar",
        "Reef",
        "Current",
        "Lagoon",
        "Harbor",
        "Dock",
        "Wharf",
        "Marina",
        "Fleet",
        "Seafarer",
        "Seafoam",
        "Shipmate",
        "Deckhand",
        "Net",
        "Clam",
        "Shellfish",
        "Booty",
        "Voyager",
        "Turtle",
        "Explorer",
        "Adventurer"
    };
    
    public PirateName GenerateUniquePirateName(string playerID)
    {
        if (playerID != string.Empty) {
            // check if already has name
            if (pirateNameDictionary.ContainsKey(playerID)) {
                return pirateNameDictionary[playerID];
            }
        }

        PirateName pirateName;

        // Check if the name is already in the list
        do {
            pirateName = GeneratePirateName();
        }
        while (pirateNames.Contains(pirateName));

        // Add the unique name to the list
        pirateNames.Add(pirateName);

        // add to dictionary
        if (playerID != string.Empty) {
            pirateNameDictionary[playerID] = pirateName;
        }

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

    void Start()
    {
        if (pirateNames == null)
        {
            pirateNames = new List<PirateName>();
        }
        if (pirateNameDictionary == null)
        {
            pirateNameDictionary = new Dictionary<string, PirateName>();
        }
        
        Debug.Log(pirateNameDictionary.Count);
    }
}