using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

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
    private UnityWebRequest sseRequest; // Request for the SSE connection
    private bool isListening = false; // Flag to track if the SSE connection is active

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

            Debug.Log("Light presets loaded into dictionary.");

            StartCoroutine(Test()); // Start the SSE connection after loading presets
        }
        else
        {
            Debug.LogError($"Failed to load presets: {jsonRequest.error}");
        }
    }

    void Start()
    {
        // Load presets from the server
        StartCoroutine(BuildPresets());
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(4f);
        UpdateLED(RFIDLed.OCCUPIED);
        yield return new WaitForSeconds(4f);
        UpdateLED(RFIDLed.BUSY);
        yield return new WaitForSeconds(4f);
        UpdateLED(RFIDLed.SUCCESS);
        yield return new WaitForSeconds(4f);
        UpdateLED(RFIDLed.OFF);
    }

    private IEnumerator EstablishSSEConnection()
    {
        sseRequest = UnityWebRequest.Get(serverUrl + "/sse");
        sseRequest.downloadHandler = new DownloadHandlerBuffer();

        // Send the request and wait for the response
        yield return sseRequest.SendWebRequest();

        if (sseRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("SSE connection established!");
            isListening = true;

            // Start listening for server-sent events
            StartCoroutine(ListenForEvents());

            StartCoroutine(Test());
        }
        else
        {
            Debug.LogError($"Failed to establish SSE connection: {sseRequest.error}");
        }
    }

    private IEnumerator ListenForEvents()
    {
        while (isListening)
        {
            // Check if there is new data in the response
            string response = sseRequest.downloadHandler.text;

            if (!string.IsNullOrEmpty(response))
            {
                // Process the received event
                Debug.Log($"Received SSE event: {response}");
                HandleServerEvent(response);

                // Clear the download buffer to avoid processing the same data repeatedly
                sseRequest.downloadHandler.Dispose();
                sseRequest.downloadHandler = new DownloadHandlerBuffer();
            }

            yield return null; // Wait for the next frame
        }
    }

    private void HandleServerEvent(string eventData)
    {
        // Example: Handle the server-sent event
        Debug.Log($"Handling server event: {eventData}");
        // Add logic to process the event data (e.g., update LED state)
    }

    private IEnumerator SendMessageToServer(string url, string messageToSend)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(messageToSend);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Message sent successfully!");
        }
        else
        {
            Debug.LogError($"Error sending message: {request.error}");
        }
    }

    public void UpdateLED(RFIDLed preset)
    {
        if (lightPresets.TryGetValue(preset, out LightPreset lightPreset))
        {
            string message = lightPreset.ToString(); // Convert the preset to a JSON string
            StartCoroutine(SendMessageToServer(serverUrl + "/lights", message));
        }
        else
        {
            Debug.LogWarning($"Preset for {preset} not found.");
        }
    }

    private void OnDestroy()
    {
        // Clean up the SSE connection when the object is destroyed
        isListening = false;
        if (sseRequest != null)
        {
            sseRequest.Dispose();
        }
        UpdateLED(RFIDLed.OFF);
    }
}