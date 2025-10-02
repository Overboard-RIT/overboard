using UnityEngine;
using TMPro;

public class ValueDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject displayText;
    private string value;
    private Color color;
    public string Value
    {
        get { return value; }
        set
        {
            this.value = value;
            displayText.GetComponent<TextMeshProUGUI>().text = value;
        }
    }

    public Color TextColor
    {
        get { return color; }
        set
        {
            color = value;
            displayText.GetComponent<TextMeshProUGUI>().color = value;
        }
    }
}
