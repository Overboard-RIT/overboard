using UnityEngine;

public class MetaConfig : MonoBehaviour
{
    private string[] playerNames = new string[2];
    private string p1Name = "Player1DummyString";
    private string p2Name = "Player2DummyString";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerNames[0] = p1Name;
        playerNames[1] = p2Name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string[] GetNames()
    {
        return this.playerNames;
    }
}
