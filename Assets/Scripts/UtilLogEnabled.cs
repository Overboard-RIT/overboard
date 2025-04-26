using UnityEngine;

public class UtilLogEnabled : MonoBehaviour
{
    private void OnEnable() {
        Debug.Log($"{gameObject.name} was enabled: {System.Environment.StackTrace}");
    }

    private void OnDisable() {
        Debug.Log($"{gameObject.name} was disabled: {System.Environment.StackTrace}");
    }
}
