using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* Author: Andrew Black
 * Since 4/17/25
 * BackWallOnboardEventManager (take a breath) holds all information related to the backwall UI. 
 * In short, this has logic for waiting on Players and updating the UI to account for this.
 * This is largely hiding/revealing elements.
 */

public class UIReferenceHolder : MonoBehaviour
{
    [Header("Scully")]
    public Image scully;
    public Image scullySpeechBubble;
    public TextMeshProUGUI scullySpeech;

    [Header("Player 1 UI")]
    public Image p1ContainerFull;
    public Image p1ContainerEmpty;
    public Image p1BoardShadow;
    public Image p1Board;
    public Image p1Icon;
    public TextMeshProUGUI p1Name;
    public Image p1WaitingText;
    public Image p1ReadyText;

    [Header("Player 2 UI")]
    public Image p2ContainerFull;
    public Image p2ContainerEmpty;
    public Image p2BoardShadow;
    public Image p2Board;
    public Image p2Icon;
    public TextMeshProUGUI p2Name;
    public Image p2WaitingText;
    public Image p2ReadyText;

    [Header("Misc")]
    public Image blueDividerLine;
    public Image letsPlaySign;
    public MetaConfig metaConfig;

    [Header("Settings")]
    public bool twoPlayer = false;

    // the only privatebools care about if the players are ready to play the game
    // which should be determined by an outside file tracking the player option
    private bool p1Ready = false;
    private bool p2Ready = false;

    // this bool "should" turn true when the players are ready
    // right now the 'effect' of this code is hardcoded in Update instead
    private bool letsPlay = false;

    // start mostly hides a bunch of elements
    // while getting the names of the players
    private void Start()
    {
        // hide Player 2 components
        SetImageAlpha(p2ContainerFull, 0f);
        SetImageAlpha(p2ContainerEmpty, 0f);
        SetImageAlpha(p2BoardShadow, 0f);
        SetImageAlpha(p2Board, 0f);
        SetImageAlpha(p2Icon, 0f);
        SetTextAlpha(p2Name, 0f);
        SetImageAlpha(p2WaitingText, 0f);
        SetImageAlpha(p2ReadyText, 0f);

        // hide Player 1 components
        SetImageAlpha(p1ContainerFull, 0f);
        SetImageAlpha(p1ContainerEmpty, 0f);
        SetImageAlpha(p1ReadyText, 0f);

        // hide "Let's Play" sign
        SetImageAlpha(letsPlaySign, 0f);

        // get player names from meta config
        string[] names = metaConfig.GetComponent<MetaConfig>().GetNames();
        if (names.Length >= 2)
        {
            p1Name.text = names[0];
            p2Name.text = names[1];
        }
    }

    // update is largely checking for player ready statuses, and calling SetImageAlpha when ready
    // it will likely be updated to make calls to move the UI along somewhere else
    // as of now, Keyboard controls are enabled to test the logic system
    private void Update()
    {
        // Keyboard Controls for 2 Player Mode and Ready states
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleTwoPlayerMode();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            SetP1Ready(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SetP2Ready(true);
        }


        // following ifs set more alphas = 0 or 1
        if (p1Ready)
        {
            SetImageAlpha(p1ContainerFull, 1f);
            SetImageAlpha(p1WaitingText, 0f);
            SetImageAlpha(p1ReadyText, 1f);
        }

        if (p2Ready)
        {
            SetImageAlpha(p2ContainerFull, 1f);
            SetImageAlpha(p2WaitingText, 0f);
            SetImageAlpha(p2ReadyText, 1f);
        }

        // this if/else is responsible for getting the letsPlaySign visible,
        // which would be the final step before the game starts.
        // right now it's hardcoded in but in the future SHOULD be utilizing the 
        // isReady function 
        if (twoPlayer)
        {
            if(p1Ready && p2Ready)
            {
                SetImageAlpha(letsPlaySign, 1f);
            }
        }
        else
        {
            if (p1Ready)
            {
                SetImageAlpha(letsPlaySign, 1f);
            }
        }
    }

    // ToggleTwoPlayerMode is what creates a large shift in visible
    // UI elements to make it more palattable for players
    private void ToggleTwoPlayerMode()
    {
        // toggle the bool
        twoPlayer = !twoPlayer;

        // reset P2 components and Player 1 empty containers when enabling two-player mode
        if (twoPlayer)
        {
            SetImageAlpha(p2ContainerFull, 0f);
            SetImageAlpha(p2ContainerEmpty, 1f);
            SetImageAlpha(p2BoardShadow, 1f);
            SetImageAlpha(p2Board, 1f);
            SetImageAlpha(p2Icon, 1f);
            SetTextAlpha(p2Name, 1f);
            SetImageAlpha(p2WaitingText, 1f);

            SetImageAlpha(p1ContainerEmpty, 1f);

            SetImageAlpha(blueDividerLine, 0f);
        }
        else
        {
            SetImageAlpha(p2ContainerFull, 0f);
            SetImageAlpha(p2ContainerEmpty, 0f);
            SetImageAlpha(p2BoardShadow, 0f);
            SetImageAlpha(p2Board, 0f);
            SetImageAlpha(p2Icon, 0f);
            SetTextAlpha(p2Name, 0f);
            SetImageAlpha(p2WaitingText, 0f);
            SetImageAlpha(p2ReadyText, 0f);

            SetImageAlpha(p1ContainerEmpty, 0f);

            SetImageAlpha(blueDividerLine, 1f);
        }
    }



    // SetP1Ready sets the Player 1 ready status to true
    // (it shouldn't be able to go to false under normal circumstances)
    private void SetP1Ready(bool isReady)
    {
        p1Ready = isReady;
    }

    // SetP2Ready sets the Player 2 ready status to true
    // (it shouldn't be able to go to false under normal circumstances)
    private void SetP2Ready(bool isReady)
    {
        p2Ready = isReady;
    }

    // SetImageAlpha takes the alpha of an image and changes it. Practically, all calls either set
    // to 0 or 1 which is a way of changing the visibility from false to true
    private void SetImageAlpha(Image img, float alpha)
    {
        if (img != null)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }

    // SetTextAlpha is the same as above but for Text
    private void SetTextAlpha(TextMeshProUGUI tmp, float alpha)
    {
        if (tmp != null)
        {
            Color c = tmp.color;
            c.a = alpha;
            tmp.color = c;
        }
    }
}