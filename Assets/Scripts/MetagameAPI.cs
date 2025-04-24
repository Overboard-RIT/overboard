using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MetagameAPI : MonoBehaviour
{
    private const string BaseUrl = "http://new-media-metagame.com/api/";
    private const string GetUrl = "band_id";
    private const string PostUrl = "prize-money/award";
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
        string url = $"{BaseUrl}{GetUrl}={UnityWebRequest.EscapeURL(rfidIdentifier)}";
        
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Player ID received: {request.downloadHandler.text}");
                scanner.SequenceSuccess();

                // do stuff with the ID
                currentPlayerID = request.downloadHandler.text;

            }
            else
            {
                Debug.LogError($"GET request failed: {request.error}");
                scanner.SequenceFailure();
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
        string url = $"{BaseUrl}{PostUrl}";
        WWWForm form = new WWWForm();
        form.AddField("interactive_slug", gameID);
        form.AddField("player_id", playerID);
        form.AddField("amount", score);

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