using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MetagameAPI : MonoBehaviour
{
    private const string BaseUrl = "http://new-media-metagame.com/api/";
    private const string GetUrl = "bands?bandId";
    private const string PostUrl = "prize-money/award";
    private const string GameID = "overboard"; // our ID in the metagame
    public string currentPlayerID; // id of the current player
    public bool apiEnabled = true; // flag to enable or disable API calls
    public RFIDScanner scanner; // reference to the RFID scanner

    // Define a class to match the JSON structure
    [System.Serializable]
    private class PlayerResponse
    {
        public string id;
        public string band_id;
        public object details;
        public string created_at;
        public int prize;

    }

    // Sends a GET request with a string parameter to retrieve a player ID
    public void GetPlayerID(string rfidIdentifier)
    {
        StartCoroutine(GetPlayerIDCoroutine(rfidIdentifier));
    }

    private IEnumerator GetPlayerIDCoroutine(string rfidIdentifier)
    {
        if (!apiEnabled)
        {
            currentPlayerID = rfidIdentifier; // use the RFID identifier as the player ID
            scanner.SequenceSuccess();
            yield break;
        }

        // TODO: replace with actual endpoint
        string url = $"{BaseUrl}{GetUrl}={UnityWebRequest.EscapeURL(rfidIdentifier)}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Player ID received: {request.downloadHandler.text}");
                string responseText = request.downloadHandler.text;

                // Parse the JSON response to retrieve the "id" value
                PlayerResponse response = JsonUtility.FromJson<PlayerResponse>(responseText);
                if (response != null && !string.IsNullOrEmpty(response.id))
                {
                    currentPlayerID = response.id;
                    //Debug.Log($"Extracted Player ID: {currentPlayerID}");
                    scanner.SequenceSuccess();
                }
                else
                {
                    Debug.LogError("Failed to extract Player ID from response.");
                    scanner.SequenceFailure();
                }
            }
            else
            {
                Debug.LogError($"GET request failed: {request.error}");
                scanner.SequenceFailure();
            }
        }
    }

    void Start()
    {
        if (currentPlayerID == null || currentPlayerID == "") currentPlayerID = ""; // reset player ID if it's null or empty
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Sends a POST request with game ID, player ID, and score
    public void PostGameData(string playerID, int score)
    {
        StartCoroutine(PostGameDataCoroutine(GameID, playerID, score));
    }

    private IEnumerator PostGameDataCoroutine(string gameID, string playerID, int score)
    {
        // TODO: replace with actual endpoint
        if (!apiEnabled || playerID == null || playerID == "")
        {
            currentPlayerID = ""; // use the RFID identifier as the player ID
            yield break;
        }

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
            currentPlayerID = ""; // reset player ID after posting data
        }

    }
}