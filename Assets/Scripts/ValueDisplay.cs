using UnityEngine;
using TMPro;

public class ValueDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject displayText;
    private string value;
    public string Value {
        get { return value; }
        set
        {
            this.value = value;
            displayText.GetComponent<TextMeshProUGUI>().text = value;
        }
    }
}
