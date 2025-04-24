using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System;
using System.Text;

// enum for possible light states
public enum RFIDLed
{
    OFF,
    ATTRACT,
    OCCUPIED,
    BUSY,
    SUCCESS,
    NOOP,
    FAILURE,
    ERROR,
    SUCCESSBLUE
}

public class RFIDScanner : MonoBehaviour
{
    public const string serverUrl = "http://nm-rfid-2.rit.edu:8001"; // HTTP address

    private ClientWebSocket webSocket;
    private CancellationTokenSource cts;
    public AudioSource successAudio;
    public MetagameAPI metagameAPI;
    public GameManager gameManager;

    // Class that holds params for each light preset
    [System.Serializable]
    private class LightPreset
    {
        public string duration;
        public string pattern;
        public string foreground;
        public string background;
        public string period;

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    [System.Serializable]
    private class LightPresetWrapper
    {
        public LightPreset OFF;
        public LightPreset ATTRACT;
        public LightPreset OCCUPIED;
        public LightPreset BUSY;
        public LightPreset SUCCESS;
        public LightPreset NOOP;
        public LightPreset FAILURE;
        public LightPreset ERROR;
        public LightPreset SUCCESSBLUE;
    }

    private Dictionary<RFIDLed, LightPreset> lightPresets = new Dictionary<RFIDLed, LightPreset>();

    void Start()
    {
        // Load presets from the server
        StartCoroutine(BuildPresets());
    }

    private IEnumerator BuildPresets()
    {
        // Get JSON from server
        UnityWebRequest jsonRequest = UnityWebRequest.Get(serverUrl + "/presets.json");
        jsonRequest.downloadHandler = new DownloadHandlerBuffer();

        // Send the request and wait for the response
        yield return jsonRequest.SendWebRequest();

        if (jsonRequest.result == UnityWebRequest.Result.Success)
        {

            string jsonResponse = jsonRequest.downloadHandler.text;

            // Parse the JSON response into the lightPresets dictionary
            LightPresetWrapper parsedPresets = JsonUtility.FromJson<LightPresetWrapper>(jsonResponse);

            // Map the parsed presets to the dictionary
            lightPresets[RFIDLed.OFF] = parsedPresets.OFF;
            lightPresets[RFIDLed.ATTRACT] = parsedPresets.ATTRACT;
            lightPresets[RFIDLed.OCCUPIED] = parsedPresets.OCCUPIED;
            lightPresets[RFIDLed.BUSY] = parsedPresets.BUSY;
            lightPresets[RFIDLed.SUCCESS] = parsedPresets.SUCCESS;
            lightPresets[RFIDLed.NOOP] = parsedPresets.NOOP;
            lightPresets[RFIDLed.FAILURE] = parsedPresets.FAILURE;
            lightPresets[RFIDLed.ERROR] = parsedPresets.ERROR;
            lightPresets[RFIDLed.SUCCESSBLUE] = parsedPresets.SUCCESSBLUE;

            ConnectToServer("ws://nm-rfid-2.rit.edu:8001/ws");
        }
        else
        {
            Debug.LogError($"Failed to load presets: {jsonRequest.error}");
        }
    }

    private IEnumerator SendMessageToRFIDServer(string url, string messageToSend)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(messageToSend);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            
        }
        else
        {
            Debug.LogError($"Error sending message: {request.error}");
        }
    }

    public IEnumerator UpdateLED(RFIDLed preset)
    {
        if (lightPresets.TryGetValue(preset, out LightPreset lightPreset))
        {
            string message = lightPreset.ToString(); // Convert the preset to a JSON string

            // Start sending the message to the server
            yield return StartCoroutine(SendMessageToRFIDServer(serverUrl + "/lights", message));
        }
        else
        {
            Debug.LogWarning($"Preset for {preset} not found.");
        }
    }

    private async Task ConnectToServer(string uri)
    {
        webSocket = new ClientWebSocket();
        cts = new CancellationTokenSource();

        Debug.Log($"Connecting to {uri}...");
        await webSocket.ConnectAsync(new Uri(uri), cts.Token);

        Debug.Log("Connected!");

        StartCoroutine(UpdateLED(RFIDLed.ATTRACT));

        // Start listening for messages
        StartCoroutine(ReceiveLoop());

    }

    private IEnumerator ReceiveLoop()
    {
        var buffer = new byte[1024 * 4];

        while (webSocket.State == WebSocketState.Open)
        {
            var receiveTask = webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);

            // Wait for the task to complete
            while (!receiveTask.IsCompleted)
            {
                yield return null;
            }

            if (receiveTask.Result.MessageType == WebSocketMessageType.Close)
            {
                Debug.Log("Server closed connection");
                yield return webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
            else
            {
                var message = Encoding.UTF8.GetString(buffer, 0, receiveTask.Result.Count);
                Debug.Log($"Message received: {message}");

                // Handle message
                if (gameManager.gameStarted) {
                    // ignore
                    yield return StartCoroutine(UpdateLED(RFIDLed.NOOP));
                }
                else {
                    yield return StartCoroutine(UpdateLED(RFIDLed.BUSY));
                    metagameAPI.GetPlayerID(message);
                }                
            }
        }
    }

    public void SequenceSuccess() {
        // Play success sound and update LED
        successAudio.Play();
        StartCoroutine(UpdateLED(RFIDLed.SUCCESS));
        StartCoroutine(UpdateLED(RFIDLed.OCCUPIED));
    }

    public void SequenceFailure() {
        StartCoroutine(UpdateLED(RFIDLed.FAILURE));
        StartCoroutine(UpdateLED(RFIDLed.ATTRACT));
    }
}