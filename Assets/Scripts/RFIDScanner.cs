using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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

            StartCoroutine(Test()); // Test sending led commands
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

    private IEnumerator SendMessageToRFIDServer(string url, string messageToSend)
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
            StartCoroutine(SendMessageToRFIDServer(serverUrl + "/lights", message));
        }
        else
        {
            Debug.LogWarning($"Preset for {preset} not found.");
        }
    }
}