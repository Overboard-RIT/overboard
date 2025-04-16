using UnityEngine;
using UnityEngine.UI;

public class TimerAnimation : MonoBehaviour
{


    public Image blueImage; // Reference to the blue image

    public Image greyImage; // Reference to the mask image

    // time is 0 - 1
    public void updateDisplay(float time)
    {

        // update the angles
        blueImage.fillAmount = time;

        if (time != 0)
        {
            time += 0.024f;
            if (time > 1)
            {
                time = 1;
            }
        }


        greyImage.fillAmount = time;

    }
}
