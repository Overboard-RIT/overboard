using TMPro;
using UnityEngine;
using Windows.Kinect;

/* Author: Andrew Black
 * 3/20/25
 * UIManager deals with everying UI in the Calibration Screen.
 * That's keeping track of Texts mostly, but involves some math
 */
public class UIManager : MonoBehaviour
{
    public Canvas uiCanvas;

    public TextMeshProUGUI leftFootText;
    public TextMeshProUGUI rightFootText;
    public TextMeshProUGUI kinectStatusText;

    public TextMeshProUGUI leftXCalibrationText;
    public TextMeshProUGUI rightXCalibrationText;
    public TextMeshProUGUI leftZCalibrationText;
    public TextMeshProUGUI rightZCalibrationText;

    private TextMeshProUGUI[] activeCalibration;
    private int currentCalibrationIndex = 0;

    private const int LEFT_X_INDEX = 0;
    private const int LEFT_Z_INDEX = 1;
    private const int RIGHT_X_INDEX = 2;
    private const int RIGHT_Z_INDEX = 3;

    public GameObject redDotLeft;
    public GameObject redDotRight;

    public PlayerPositionDetection kinect;

    private const string DEFAULT_LEFT_FOOT_TEXT = "Left Foot: ";
    private const string DEFAULT_RIGHT_FOOT_TEXT = "Right Foot: ";
    private const string DEFAULT_KINECT_STATUS_TEXT = "Kinect Status: ";

    private bool kinectStatus = true;

    private PlayerPositionDetection.PlayerPosition playerPosition;

    private Vector3 canvasPos;

    private Vector3 leftFootCalibration = new Vector3(0, 0);
    private Vector3 rightFootCalibration = new Vector3(0, 0);

    private Config config;

    // instantiate some values, nothing crazy
    // calls UpdateActiveClibrationColor after instantiating array to give the first
    // index an orange hue for readability
    void Start()
    {
        this.canvasPos = uiCanvas.transform.position;

        activeCalibration = new[] {
            leftXCalibrationText,
            leftZCalibrationText,
            rightXCalibrationText,
            rightZCalibrationText
        };
        UpdateActiveCalibrationColor();
    }
    
    // couple things happening here
    // First, we are looking for a config file to send our updates to
    // then, we are allowing the user to cycle between the Calibration nodes
    // they want to update.
    // Finally, we are updating the positions of our Red Dots on the scene to match the calibration
    void Update()
    {
        if(this.config == null)
        {
            this.config = FindFirstObjectByType<Config>();
        }

        // Tab is the main control for cycling here
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // reset previous color
            activeCalibration[currentCalibrationIndex].color = Color.white;

            // switch to the next calibration text, looping around
            currentCalibrationIndex = (currentCalibrationIndex + 1) % activeCalibration.Length;

            UpdateActiveCalibrationColor();
        }

        // this is where we are allowing the user to change the text
        // note that 'anyKeyDown' is a bit misleading as we are parsing out for floats only
        if (Input.anyKeyDown)
        {
            string inputString = Input.inputString;

            // Allow numbers, decimals, negative sign, and backspace
            if (float.TryParse(inputString, out _) || inputString == "." || inputString == "-" || Input.GetKeyDown(KeyCode.Backspace))
            {
                TextMeshProUGUI currentText = activeCalibration[currentCalibrationIndex];

                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    // prevent deleting the leading "X:" or "Z:"
                    if (currentText.text.Length > 1)
                    {
                        currentText.text = currentText.text.Substring(0, currentText.text.Length - 1);
                    }

                    // check if text only contains 'X:' or 'Z:', reset calibration value to 0
                    if (currentText.text.Length <= 2)
                    {
                        switch (currentCalibrationIndex)
                        {
                            // note that our 'Z' values are changing the 'Y' calibrations in THIS SCENE ONLY
                            // this is due to the way I setup the calibration screen on a Panel, which doesn't utilize the
                            // Z value properly as opposed to the gameplay scene. Don't get confused - the config gets updated correctly
                            case LEFT_X_INDEX:
                                leftFootCalibration.x = 0;
                                break;
                            case LEFT_Z_INDEX:
                                leftFootCalibration.y = 0;
                                break;
                            case RIGHT_X_INDEX:
                                rightFootCalibration.x = 0;
                                break;
                            case RIGHT_Z_INDEX:
                                rightFootCalibration.y = 0;
                                break;
                        }
                    }
                }
                else
                {
                    currentText.text += inputString;
                }

                // Extract number part by removing the leading 'X' or 'Z'
                string numberPart = currentText.text.Substring(1).Trim(); // Skip the first character (X or Z)

                if (float.TryParse(numberPart, out float value))
                {
                    switch (currentCalibrationIndex)
                    {
                        case LEFT_X_INDEX:
                            leftFootCalibration.x = value;
                            break;
                        case LEFT_Z_INDEX:
                            leftFootCalibration.y = value;
                            break;
                        case RIGHT_X_INDEX:
                            rightFootCalibration.x = value;
                            break;
                        case RIGHT_Z_INDEX:
                            rightFootCalibration.y = value;
                            break;
                    }
                }
            }
        }

        // provided the kinect is working, we need its position.
        // in fact, without it the whole thing is a bit pointless, eh?
        if (kinect != null)
        {
            this.playerPosition = kinect.GetActivePositions()[0];
            UpdateDotPositions();
            UpdateText();
        }

        UpdateConfig();
    }

    // simply changes the active calibrator to orange
    private void UpdateActiveCalibrationColor()
    {
        activeCalibration[currentCalibrationIndex].color = new Color(1.0f, 0.5f, 0.0f); // Orange
    }

    // this is the logic for moving the dots on the screen
    private void UpdateDotPositions()
    {
        // first, the newPosition of the dots is based on the playerPosition's, and then the new calibration
        Vector3 newPosition = this.playerPosition.leftFoot + leftFootCalibration;

        // then, the values we care about (remember Y = Z) are multiplied by a value of +/- 110
        // this number is picked because it's the distance of two graphics representing feet on the Panel from the center
        newPosition.x *= -110.0f;
        newPosition.y *= -110.0f;
        newPosition += canvasPos;
        redDotLeft.transform.position = newPosition;

        newPosition = this.playerPosition.rightFoot + rightFootCalibration;
        newPosition.x *= 110.0f;
        newPosition.y *= 110.0f;
        newPosition += canvasPos;
        redDotRight.transform.position = newPosition;
    }

    // updates the text, nothing crazy
    private void UpdateText()
    {
        leftFootText.text = DEFAULT_LEFT_FOOT_TEXT + playerPosition.leftFoot.ToString();
        rightFootText.text = DEFAULT_RIGHT_FOOT_TEXT + playerPosition.rightFoot.ToString();
        kinectStatusText.text = DEFAULT_KINECT_STATUS_TEXT + (kinectStatus ? "Connected" : "Disconnected");
    }

    private void UpdateConfig()
    {
        if(this.config != null)
        {
            this.config.UpdateCalibration(
                new Vector3(leftFootCalibration.x, leftFootCalibration.z, leftFootCalibration.y),
                new Vector3(rightFootCalibration.x, rightFootCalibration.z, rightFootCalibration.y)
                );

        }
    }
}
