using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MetagameAPI : MonoBehaviour
{
    private const string BaseUrl = "https://your-api-url.com"; // TODO Replace
    private const string GameID = "overboard"; // our ID in the metagame
    private string currentPlayerID = string.Empty; // id of the current player
    public RFIDScanner scanner; // reference to the RFID scanner

    // Sends a GET request with a string parameter to retrieve a player ID
    public void GetPlayerID(string rfidIdentifier)
    {
        StartCoroutine(GetPlayerIDCoroutine(rfidIdentifier));
    }

    private IEnumerator GetPlayerIDCoroutine(string rfidIdentifier)
    {
        // TODO: replace with actual endpoint
        string url = $"{BaseUrl}/getPlayerID?input={UnityWebRequest.EscapeURL(rfidIdentifier)}";
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Player ID received: {request.downloadHandler.text}");

                // do stuff with the ID
                currentPlayerID = request.downloadHandler.text;

            }
            else
            {
                Debug.LogError($"GET request failed: {request.error}");
            }
        }
    }

    // Sends a POST request with game ID, player ID, and score
    public void PostGameData(string playerID, int score)
    {
        StartCoroutine(PostGameDataCoroutine(GameID, playerID, score));
    }

    private IEnumerator PostGameDataCoroutine(string gameID, string playerID, int score)
    {
        // TODO: replace with actual endpoint
        string url = $"{BaseUrl}/postGameData";
        WWWForm form = new WWWForm();
        form.AddField("gameID", gameID);
        form.AddField("playerID", playerID);
        form.AddField("score", score);

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("POST request successful.");
            }
            else
            {
                Debug.LogError($"POST request failed: {request.error}");
            }
        }
    }
}