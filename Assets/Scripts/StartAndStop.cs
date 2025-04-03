using UnityEngine;
using UnityEngine.UI;

public class StartAndStop : MonoBehaviour
{
    public float fadeDuration = 0.2f;
    public float fadeDelay = 0.2f;
    public bool show = false;

    private bool isFading = false;
    private float fadeStartedAt;

    void OnValidate()
    {
        if (show)
        {
            show = false;
            Show();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        isFading = true;
        GetComponent<Image>().color = Color.white;
        fadeStartedAt = Time.time;
    }

    public void Update()
    {
        if (isFading && Time.time - fadeStartedAt > fadeDelay)
        {
            GetComponent<Image>().color = new Color(
                1,
                1,
                1,
                Mathf.Lerp(1, 0, (Time.time - fadeStartedAt - fadeDelay) / fadeDuration)
            );
        }
        if (isFading && Time.time - fadeStartedAt > fadeDuration + fadeDelay)
        {
            isFading = false;
            gameObject.SetActive(false);
        }
    }
}
