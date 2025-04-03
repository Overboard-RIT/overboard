using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    public GameObject headingText;
    public GameObject bodyText;
    public string Heading
    {
        set
        {
            headingText.GetComponent<TextMeshProUGUI>().text = value;
        }
    }
    public string Body
    {
        set
        {
            bodyText.GetComponent<TextMeshProUGUI>().text = value;
        }
    }
}
