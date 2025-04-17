using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("Player 2 UI")]
    public Image p2ContainerFull;
    public Image p2ContainerEmpty;
    public Image p2BoardShadow;
    public Image p2Board;
    public Image p2Icon;
    public TextMeshProUGUI p2Name;

    [Header("Misc")]
    public Image blueDividerLine;
    public Image letsPlaySign;
    public MetaConfig metaConfig;

    [Header("Settings")]
    public bool twoPlayer = false;

    private bool p1Ready = false;
    private bool p2Ready = false;
    private bool letsPlay = false;

    private void Start()
    {
        // Hide Player 2 components
        SetImageAlpha(p2ContainerFull, 0f);
        SetImageAlpha(p2ContainerEmpty, 0f);
        SetImageAlpha(p2BoardShadow, 0f);
        SetImageAlpha(p2Board, 0f);
        SetImageAlpha(p2Icon, 0f);
        SetTextAlpha(p2Name, 0f);

        // Hide Player 1 container overlays
        SetImageAlpha(p1ContainerFull, 0f);
        SetImageAlpha(p1ContainerEmpty, 0f);

        // Hide "Let's Play" sign
        SetImageAlpha(letsPlaySign, 0f);

        // Get player names from config
        string[] names = metaConfig.GetComponent<MetaConfig>().GetNames();
        if (names.Length >= 2)
        {
            p1Name.text = names[0];
            p2Name.text = names[1];
        }
    }
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

        /*// When 2PlayerMode is enabled, set components to visible
        if (twoPlayer)
        {
            // Reset P2 UI and P1 empty container
            SetImageAlpha(p2ContainerFull, 0f);
            SetImageAlpha(p2ContainerEmpty, 0f); 
            SetImageAlpha(p2BoardShadow, 1f);
            SetImageAlpha(p2Board, 1f);
            SetImageAlpha(p2Icon, 1f);
            SetTextAlpha(p2Name, 1f);

            // Set P1 empty container to full
            SetImageAlpha(p1ContainerEmpty, 1f);

            // Reset blue divider line
            SetImageAlpha(blueDividerLine, 0f);

            // Set "Let's Play" sign visibility
            if (p1Ready && p2Ready)
            {
                SetImageAlpha(letsPlaySign, 1f);
            }
        }*/

        // If P1 is ready, set the alpha for P1's full container to full
        if (p1Ready)
        {
            SetImageAlpha(p1ContainerFull, 1f);
        }

        // If P2 is ready, set the alpha for P2's full container to full
        if (p2Ready)
        {
            SetImageAlpha(p2ContainerFull, 1f);
        }

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

    // Toggle the two-player mode
    private void ToggleTwoPlayerMode()
    {
        twoPlayer = !twoPlayer;

        // Reset P2 components and Player 1 empty containers when enabling two-player mode
        if (twoPlayer)
        {
            SetImageAlpha(p2ContainerFull, 0f);
            SetImageAlpha(p2ContainerEmpty, 1f);
            SetImageAlpha(p2BoardShadow, 1f);
            SetImageAlpha(p2Board, 1f);
            SetImageAlpha(p2Icon, 1f);
            SetTextAlpha(p2Name, 1f);

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

            SetImageAlpha(p1ContainerEmpty, 0f);


            SetImageAlpha(blueDividerLine, 1f);
        }
    }

    // Set Player 1 ready status
    private void SetP1Ready(bool isReady)
    {
        p1Ready = isReady;
    }

    // Set Player 2 ready status
    private void SetP2Ready(bool isReady)
    {
        p2Ready = isReady;
    }

    // Utility: Set image alpha
    private void SetImageAlpha(Image img, float alpha)
    {
        if (img != null)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }

    // Utility: Set text alpha
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