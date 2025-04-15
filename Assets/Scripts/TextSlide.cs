using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class FloatingTextUI : MonoBehaviour
{
    private Vector3 startPos;
    public UnityEngine.UI.Text uiText;
    private Color startColor;

    public float fadeInTime = 0.5f;
    public float visibleTime = 1f;
    public float fadeOutTime = 0.5f;
    public float moveUpDistance = 30f;

    public AudioSource splash;
    public AudioSource cowbell;

    private void Start()
    {
        startPos = uiText.rectTransform.anchoredPosition;
        startColor = new Color(1, 0, 0.26f, 0); // Red text, initially invisible
        uiText.color = startColor;
    }

    public void ShowText()
    {
        StopAllCoroutines(); // Ensure no overlapping animations
        StartCoroutine(AnimateText());
    }

    private IEnumerator AnimateText()
    {
        splash.Play();
        float timer = 0f;
        Vector3 endPos = startPos + Vector3.up * moveUpDistance;

        // Fade In & Move Up
        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeInTime;
            uiText.color = new Color(1, 0.30f, 0.51f, progress);
            uiText.rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, progress);
            yield return null;
        }

        // Stay Visible
        yield return new WaitForSeconds(visibleTime);

        // Fade Out
        timer = 0f;
        cowbell.Play();
        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeOutTime;
            uiText.color = new Color(1, 0, 0, 1 - progress);
            yield return null;
        }

        // Reset text (invisible & original position)
        uiText.color = startColor;
        uiText.rectTransform.anchoredPosition = startPos;
    }
}
