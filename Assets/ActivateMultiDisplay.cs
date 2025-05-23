// taken from https://docs.unity3d.com/Manual/multidisplay.html

using UnityEngine;
using System.Collections;

public class ActivateAllDisplays : MonoBehaviour
{
    void Start()
    {
        //Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
        // Check if additional displays are available and activate each.

        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }

        // debugging for determining which display is what
      

        for (int i = 0; i < Display.displays.Length; i++)
        {
            //Debug.Log($"Display {i}: {Screen.width}x{Screen.height}");
            Display.displays[i].Activate();
        }
    }

    void Update()
    {

    }
}

