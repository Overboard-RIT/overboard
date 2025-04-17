using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class ScullyLoader : MonoBehaviour
{
    public string assetBundlePath = "Assets/AssetBundles/video_bundle";
    private AssetBundle videoBundle;

    public enum ScullyCategory
    {
        StepOntoTheRaft,
        GameBand,
        HowToPlay,
        ChooseDifficulty,
        Idle,
        Overboard,
        Banter,
        RoundEnd,
        LowScore,
        NormalScore,
        HighScore,
        Rank,
        EndGame
    }

    // Your categorized name arrays as strings
    public VideoPlayer source;
    public string[] stepOntoTheRaft;
    public string[] gameBand;
    public string[] howToPlay;
    public string[] chooseDifficulty;
    public string[] idle;
    public string[] overboard;
    public string[] banter;
    public string[] roundEnd;
    public string[] lowScore;
    public string[] normalScore;
    public string[] highScore;
    public string[] rank;
    public string[] endGame;

    // Your categorized VideoClip arrays
    private VideoClip[] stepOntoTheRaftClips;
    private VideoClip[] gameBandClips;
    private VideoClip[] howToPlayClips;
    private VideoClip[] chooseDifficultyClips;
    private VideoClip[] idleClips;
    private VideoClip[] overboardClips;
    private VideoClip[] banterClips;
    private VideoClip[] roundEndClips;
    private VideoClip[] lowScoreClips;
    private VideoClip[] normalScoreClips;
    private VideoClip[] highScoreClips;
    private VideoClip[] rankClips;
    private VideoClip[] endGameClips;

    void Start()
    {
        LoadVideoBundle();
        LoadAllClips();
    }

    private void LoadScully(VideoClip clip)
    {
        if (clip != null)
        {
            source.clip = clip;
            source.Play();
        }
        else
        {
            Debug.LogError("Clip is null!");
        }
    }

    private VideoClip PlayScullyGetVideoClip(ScullyCategory category)
    {
        VideoClip[] clips = null;

        switch (category)
        {
            case ScullyCategory.StepOntoTheRaft:
                clips = stepOntoTheRaftClips;
                break;
            case ScullyCategory.GameBand:
                clips = gameBandClips;
                break;
            case ScullyCategory.HowToPlay:
                clips = howToPlayClips;
                break;
            case ScullyCategory.ChooseDifficulty:
                clips = chooseDifficultyClips;
                break;
            case ScullyCategory.Idle:
                clips = idleClips;
                break;
            case ScullyCategory.Overboard:
                clips = overboardClips;
                break;
            case ScullyCategory.Banter:
                clips = banterClips;
                break;
            case ScullyCategory.RoundEnd:
                clips = roundEndClips;
                break;
            case ScullyCategory.LowScore:
                clips = lowScoreClips;
                break;
            case ScullyCategory.NormalScore:
                clips = normalScoreClips;
                break;
            case ScullyCategory.HighScore:
                clips = highScoreClips;
                break;
            case ScullyCategory.Rank:
                clips = rankClips;
                break;
            case ScullyCategory.EndGame:
                clips = endGameClips;
                break;
        }

        return PickRandomClip(clips);
    }

    public void PlayScully(ScullyCategory category)
    {
        VideoClip clip = PlayScullyGetVideoClip(category);
        LoadScully(clip);
    }

    public void PlayScullyLooping(ScullyCategory category, float loopDelay)
    {
        StartCoroutine(LoopScullyCoroutine(category, loopDelay));
    }

    private IEnumerator LoopScullyCoroutine(ScullyCategory category, float loopDelay)
    {
        while (true)
        {
            yield return PlayScullyCoroutine(category);
            yield return new WaitForSeconds(loopDelay);
        }
    }

    public IEnumerator PlayScullyCoroutine(ScullyCategory category)
    {
        VideoClip clip = PlayScullyGetVideoClip(category);
        LoadScully(clip);
        yield return new WaitForSeconds((float)clip.length);
    }
    private VideoClip PickRandomClip(VideoClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        return clips[randomIndex];
    }

    private void LoadVideoBundle()
    {
        if (videoBundle == null)
        {
            videoBundle = AssetBundle.LoadFromFile(assetBundlePath);
            if (videoBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle!");
            }
        }
    }

    private void LoadAllClips()
    {
        // Populate VideoClip arrays by loading clips from the string arrays
        stepOntoTheRaftClips = LoadCategory(stepOntoTheRaft);
        gameBandClips = LoadCategory(gameBand);
        howToPlayClips = LoadCategory(howToPlay);
        chooseDifficultyClips = LoadCategory(chooseDifficulty);
        idleClips = LoadCategory(idle);
        overboardClips = LoadCategory(overboard);
        banterClips = LoadCategory(banter);
        roundEndClips = LoadCategory(roundEnd);
        lowScoreClips = LoadCategory(lowScore);
        normalScoreClips = LoadCategory(normalScore);
        highScoreClips = LoadCategory(highScore);
        rankClips = LoadCategory(rank);
        endGameClips = LoadCategory(endGame);

        Debug.Log("All clips loaded.");
    }

    // Helper function to load VideoClip array for a given category
    private VideoClip[] LoadCategory(string[] category)
    {
        if (category == null || category.Length == 0) return null;

        VideoClip[] clips = new VideoClip[category.Length];

        for (int i = 0; i < category.Length; i++)
        {
            string clipName = category[i];  // Clip name from the string array
            clips[i] = videoBundle.LoadAsset<VideoClip>(clipName);

            if (clips[i] == null)
            {
                Debug.LogWarning($"Clip '{clipName}' not found in bundle.");
            }
        }

        return clips;
    }
}
