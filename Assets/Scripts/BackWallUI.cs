using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Xml.Linq;

public class BackWallUI : MonoBehaviour
{
    public GameObject playersPanel;
    public GameObject timer;
    public GameObject score;
    public GameObject player1;
    public GameObject player2;
    public GameObject speechBubble;
    public GameObject speechBubblePanel;
    public GameObject idlePanel;
    private List<OverboardPlayer> players = new List<OverboardPlayer>();

    public GameObject scullyPlatform;
    public GameObject scullyNeutral;
    public GameObject scullyPoint;

    public AudioSource SquawkSound;

    [Header("Player 1 Onboarding UI")]
    public GameObject onboardingPanel;
    public Image p1ContainerFull;
    public Image p1ContainerFull2;
    public Image p1ContainerEmpty;
    public Image p1BoardShadow;
    public Image p1Board;
    public Image p1Icon;
    public TextMeshProUGUI p1Name;
    public Image p1WaitingText;
    public Image p1ReadyText;
    public Image blueDividerLine;
    public Image letsPlaySign;

    [Header("Settings")]
    public Image diffCasual;
    public Image diffExpert;

    private Scully scullyScript;

    // private stuff dealing with onboarding, currentDifficulty
    // and if P1 is ready to play
    private string currentDiff = "casual";
    private bool p1Ready = false;

    private OverboardPlayer example = new OverboardPlayer("Player 1", 0, 0);

    public struct OverboardPlayer
    {
        public string name;
        public int score;
        public int metagameScore;
        public OverboardPlayer(string name, int score, int metagameScore)
        {
            this.name = name;
            this.score = score;
            this.metagameScore = metagameScore;
        }
    }

    public void SetTimer(int time)
    {
        string timeText;
        if (time == 60)
        {
            timeText = "1:00";
        }
        else
        if (time < 10)
        {
            timeText = "0:0" + time.ToString();
        }
        else
        {
            timeText = "0:" + time.ToString();
        }
        timer.GetComponent<TextMeshProUGUI>().text = timeText;
    }

    public void SetScore(int newScore)
    {
        score.GetComponent<TextMeshProUGUI>().text = newScore.ToString() + " pts";
    }

    public void AddPlayer(OverboardPlayer player)
    {
        players.Add(player);

        if (players.Count == 1)
        {
            GameObject newPlayer = Instantiate(player1, playersPanel.transform);
            newPlayer.GetComponent<PlayerUI>().PlayerNumber = players.Count;
            newPlayer.GetComponent<PlayerUI>().Player = player;
        }
        else if (players.Count == 2)
        {
            GameObject newPlayer = Instantiate(player2, playersPanel.transform);
            newPlayer.GetComponent<PlayerUI>().PlayerNumber = players.Count;
            newPlayer.GetComponent<PlayerUI>().Player = player;
        }
    }

    public void Squawk(string heading, string body)
    {
        foreach (Transform child in speechBubblePanel.transform)
        {
            Destroy(child.gameObject);
        }

        SquawkSound.Play();

        GameObject newBubble = Instantiate(speechBubble, speechBubblePanel.transform);
        newBubble.GetComponent<SpeechBubble>().Heading = heading;
        newBubble.GetComponent<SpeechBubble>().Body = body;
    }

    public void Quiet()
    {
        foreach (Transform child in speechBubblePanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // public void HideScully() {
    //     scullyPlatform.SetActive(false);
    //     scullyNeutral.SetActive(false);
    //     scullyPoint.SetActive(false);
    // }

    // public void ShowScullyPoint() {
    //     scullyPlatform.SetActive(true);
    //     scullyPoint.SetActive(true);
    //     scullyNeutral.SetActive(false);
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        idlePanel.SetActive(false);
        onboardingPanel.SetActive(false);
        score.GetComponent<TextMeshProUGUI>().text = "0 pts";
        // AddPlayer(example);
        //AddPlayer(example);
        Squawk("Ahoy There!", "Kindly stand upon me trusty raft to start the game!");
    }

    // does the UI stuff for Onboarding, namely hiding 
    // members of the Backwall Panel(s) that we don't want to see
    public void StartOnboarding()
    {
        idlePanel.SetActive(false);
        onboardingPanel.SetActive(true);

        scullyScript.SquawkRaft();

        // hide Player 2 components
        /*SetImageAlpha(p2ContainerFull, 0f);
        SetImageAlpha(p2ContainerEmpty, 0f);
        SetImageAlpha(p2BoardShadow, 0f);
        SetImageAlpha(p2Board, 0f);
        SetImageAlpha(p2Icon, 0f);
        SetTextAlpha(p2Name, 0f);
        SetImageAlpha(p2WaitingText, 0f);
        SetImageAlpha(p2ReadyText, 0f);*/

        // hide Player 1 components
        SetImageAlpha(p1ContainerFull, 0f);
        SetImageAlpha(p1ContainerFull2, 0f);
        SetImageAlpha(p1ContainerEmpty, 0f);
        SetImageAlpha(p1ReadyText, 0f);

        // hide "Let's Play" sign
        SetImageAlpha(letsPlaySign, 0f);

        // hide Expert difficulty
        SetImageAlpha(diffExpert, 0f);

        // get player names from meta config
        /*string[] names = metaConfig.GetComponent<MetaConfig>().GetNames();
        if (names.Length >= 2)
        {
            p1Name.text = names[0];
            p2Name.text = names[1];
        }*/

    }

    private void ToggleDifficulty()
    {
        if (this.currentDiff == "casual")
        {
            SetImageAlpha(diffCasual, 0f);
            SetImageAlpha(diffExpert, 1f);
            currentDiff = "expert";
        }
        else
        {
            SetImageAlpha(diffCasual, 1f);
            SetImageAlpha(diffExpert, 0f);
            currentDiff = "casual";
        }
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

    void Start() {
        GoIdle();
        this.scullyScript = scullyNeutral.GetComponent<Scully>();
    }

    public void GoIdle()
    {
        idlePanel.SetActive(true);
        onboardingPanel.SetActive(false);
        foreach (Transform child in playersPanel.transform)
        {
            Destroy(child.gameObject);
        }
        players.Clear();
    }

    private void Update()
    {
        // key commands are for testing purposes

        // change UI to match P1 being ready
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayerReady();
        }

        // call the onboarding process, which changes the UI
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartOnboarding();
        }

        // change the difficulty
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleDifficulty();
        }

        // start the game (if p1Ready)
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (p1Ready)
            {
                StartGame();
            }
        }
    }

    private void PlayerReady()
    {
        SetImageAlpha(p1ContainerFull, 1f);
        SetImageAlpha(p1ContainerFull2, 1f);
        SetImageAlpha(p1WaitingText, 0f);
        SetImageAlpha(p1ReadyText, 1f);
        SetImageAlpha(p1BoardShadow, 0f);

        // sets the game
        SetImageAlpha(letsPlaySign, 1f);
    }

}
